using Hand.Collections;
using Hand.ConcurrentCollections;
using Hand.Job;
using Hand.States;
using Hand.Structural;
using Hand.Tasks.Internal;
using System.Collections.Concurrent;
using TaskItem = Hand.States.IState<bool>;

namespace Hand.Tasks;

/// <summary>
/// 队列调度器
/// </summary>
public class QueueTaskScheduler
    : TaskScheduler, IQueueProcessor<TaskItem>
{
    #region 配置
    private readonly LinkedListAdaptQueue<TaskItem> _queue = new();
    private readonly ConcurrentDictionary<Task, TaskItem> _tasks = [];
    /// <summary>
    /// 待执行任务数量
    /// </summary>
    public int Count
        => _queue.Count;
    /// <summary>
    /// 队列
    /// </summary>
    public IQueue<TaskItem> Queue
        => _queue;
    #endregion
    #region TaskScheduler
    /// <inheritdoc />
    protected override IEnumerable<Task> GetScheduledTasks()
        => _tasks.Keys;
    /// <inheritdoc />
    protected override void QueueTask(Task task)
    {
        var state = new TaskState(task);
        _queue.Enqueue(state);
        _tasks[task] = state;
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
        if (_tasks.TryRemove(task, out var state))
        {
            _queue.Remove(state);
            return true;
        }
        return false;
    }

    #endregion
    #region IQueueProcessor<Task>
    async void IQueueProcessor<TaskItem>.Run(IQueue<TaskItem> queue, ThreadJobService<TaskItem> service, CancellationToken token)
    {
        while (queue.TryDequeue(out var item))
        {
            if (service.Activate(item))
            {
                if (item is IJobItem job)
                {
                    try
                    {
                        job.Run();
                    }
                    catch (Exception ex)
                    {
                        OnException(job, ex);
                    }
                }
                else if (item is IAsyncJobItem asyncJob)
                {
                    try
                    {
                        await asyncJob.RunAsync(token);
                    }
                    catch (Exception ex)
                    {
                        OnException(asyncJob, ex);
                    }
                }
                else if (item is IWrapper<Task> task)
                {
                    var original = task.Original;
                    _tasks.TryRemove(original, out _);
                    TryExecuteTask(original);
                }
            }
            else
            {
                if (item is ICancelable cancelable)
                    OnCancel(cancelable);
                break;
            }
            if (token.IsCancellationRequested)
                break;
        }
        // 线程用完释放(回收)
        service.Dispose();
    }
    #endregion
    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="callBack"></param>
    /// <param name="ex"></param>

    private static void OnException(IExceptionable callBack, Exception ex)
    {
        try
        {
            callBack.OnException(ex);
        }
        catch { }
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="cancelable"></param>
    private static void OnCancel(ICancelable cancelable)
    {
        try
        {
            cancelable.OnCancel();
        }
        catch { }
    }
}
