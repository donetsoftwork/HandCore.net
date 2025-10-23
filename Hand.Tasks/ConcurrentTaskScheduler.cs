using Hand.Concurrent;
using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// 并发调度器
/// </summary>
/// <param name="concurrency"></param>
public class ConcurrentTaskScheduler(ConcurrentControl concurrency)
    : TaskScheduler, IProcessor
{
    #region 配置
    /// <summary>
    /// 任务
    /// </summary>
    private readonly LinkedList<Task> _tasks = new();
    /// <summary>
    /// 并发控制
    /// </summary>
    protected readonly ConcurrentControl _concurrency = concurrency;
    /// <summary>
    /// 并发上限
    /// </summary>
    public override int MaximumConcurrencyLevel
        => _concurrency.Limit;
    /// <summary>
    /// 待执行任务数量
    /// </summary>
    public int TaskCount
        => _tasks.Count;
    #endregion
    #region TaskScheduler
    /// <inheritdoc />
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        lock (_tasks)
        {
            return [.. _tasks];
        }
    }
    /// <inheritdoc />
    protected override void QueueTask(Task task)
    {
        lock (_tasks)
        {
            _tasks.AddLast(task);
        }
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="task"></param>
    public void AddTask(Task task)
    {
        switch (task.Status)
        {
            case TaskStatus.RanToCompletion:
            case TaskStatus.Faulted:
            case TaskStatus.Canceled:
                // 执行完成的忽略
                break;
            case TaskStatus.Created:
                // 尚未调度的启动
                // 实际是添加,但不能直接调用QueueTask
                task.Start(this);
                break;
            default:
                // 其他增加占用并发配额
                if (_concurrency.Increment())
                {
                    task.ContinueWith(t =>
                    {
                        // 执行完成返回并发配额
                        _concurrency.Decrement();
                    }, this);
                }
                else
                {
                    task.ContinueWith(t => { }, this);
                }
                break;
        }
    }
    /// <inheritdoc />
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        if (taskWasPreviouslyQueued)
            TryDequeue(task);
        return TryExecuteTask(task);
    }
    /// <inheritdoc />
    protected override bool TryDequeue(Task task)
    {
        if (_tasks.Contains(task))
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }
        return false;
    }
    #endregion
    #region IProcessor
    /// <inheritdoc />
    public bool Run()
    {
        if (_tasks.Count == 0)
            return false;
        // 占用并发配额
        if (_concurrency.Increment())
        {
            var state = RunCore();
            // 执行结束返回并发配额
            _concurrency.Decrement();
            return state;
        }
        return false;
    }
    /// <summary>
    /// 实际执行
    /// </summary>
    /// <returns></returns>
    private bool RunCore()
    {
        LinkedListNode<Task> first = null;
        lock (_tasks)
        {
            first = _tasks.First;
            if (first is null)
                return false;
            _tasks.Remove(first);
        }
        try
        {
            // 不能向外层抛异常,会打断外层任务调度服务
            TryExecuteTask(first.Value);
        }
        catch { }
        return true;
    }
    #endregion
}
