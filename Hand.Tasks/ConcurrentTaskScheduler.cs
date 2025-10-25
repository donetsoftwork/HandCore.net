using Hand.Concurrent;
using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// 并发调度器
/// </summary>
/// <param name="options"></param>
public class ConcurrentTaskScheduler(ConcurrentOptions options)
    : TaskScheduler, IProcessor
{
    #region 配置
    /// <summary>
    /// 任务
    /// </summary>
    private readonly LinkedList<Task> _tasks = new();
#if NET9_0_OR_GREATER
    private readonly Lock _taskLock = new();
#endif
    /// <summary>
    /// 并发控制
    /// </summary>
    protected readonly ConcurrentControl _control = new(options.ConcurrencyLevel);
    /// <summary>
    /// 当前并发数
    /// </summary>
    public int Concurrency
        => _control.Count;
    /// <summary>
    /// 并发上限
    /// </summary>
    public override int MaximumConcurrencyLevel
        => _control.Limit;
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
#if NET9_0_OR_GREATER
        lock (_taskLock)
#else
        lock (_tasks)
#endif
        {
            return [.. _tasks];
        }
    }
    /// <inheritdoc />
    protected override void QueueTask(Task task)
    {
#if NET9_0_OR_GREATER
        lock (_taskLock)
#else
        lock (_tasks)
#endif
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
                if (_control.Increment())
                {
                    task.ContinueWith(t =>
                    {
                        // 执行完成返回并发配额
                        _control.Decrement();
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
#if NET9_0_OR_GREATER
            lock (_taskLock)
#else
            lock (_tasks)
#endif
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
        if (_control.Increment())
        {
            var state = RunCore();
            // 执行结束返回并发配额
            _control.Decrement();
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
#if NET9_0_OR_GREATER
        lock (_taskLock)
#else
        lock (_tasks)
#endif
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
