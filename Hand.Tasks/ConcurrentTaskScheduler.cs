using Hand.Concurrent;
using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// 并发调度器
/// </summary>
/// <param name="options"></param>
public class ConcurrentTaskScheduler(ConcurrentOptions options)
    : TaskScheduler, IProcessor, IQueueProcessor<Task>
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
    private readonly ConcurrentControl _control = new(options.ConcurrencyLevel);
    /// <summary>
    /// 并发控制
    /// </summary>
    internal ConcurrentControl Control
        => _control;
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
    #region ICollectionProcessor<Task>
    /// <inheritdoc />
    public bool TryTake(out Task item)
    {
        LinkedListNode<Task> first = null;
        var state = false;
        if (_tasks.Count > 0)
        {
            // 扣除并发配额
            if (_control.TryIncrement())
            {
#if NET9_0_OR_GREATER
                lock (_taskLock)
#else
                lock (_tasks)
#endif
                {
                    first = _tasks.First;
                    if (first is not null)
                    {
                        state = true;
                        _tasks.Remove(first);
                    }                        
                }
                // 不需要执行,返还并发配额
                if (!state)
                    _control.Increment();
            }
        }
        if (state)
        {
            item = first.Value;
            return true;
        }
        item = default;
        return false;
    }
    /// <inheritdoc />
    public bool Run(ref Task instance)
    {
        var state = TryExecuteTask(instance);
        // 执行结束返还并发配额
        _control.Decrement();
        return state;
    }
    #endregion
    #region IProcessor
    /// <inheritdoc />
    public bool Run()
    {
        if(TryTake(out var task))
        {
            Run(ref task);
            return true;
        }
        return false;
    }
    #endregion
}
