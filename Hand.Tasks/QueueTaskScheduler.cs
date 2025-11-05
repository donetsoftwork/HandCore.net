using Hand.Collections;
using Hand.ConcurrentCollections;
using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// 队列调度器
/// </summary>
public class QueueTaskScheduler
    : TaskScheduler, IQueueProcessor<Task>
{
    #region 配置
    private readonly LinkedListAdaptQueue<Task> _queue = new();
    /// <summary>
    /// 待执行任务数量
    /// </summary>
    public int Count
        => _queue.Count;
    /// <summary>
    /// 队列
    /// </summary>
    public IQueue<Task> Queue
        => _queue;
    #endregion
    #region TaskScheduler
    /// <inheritdoc />
    protected override IEnumerable<Task> GetScheduledTasks()
        => _queue.Items;
    /// <inheritdoc />
    protected override void QueueTask(Task task)
        => _queue.Enqueue(task);
    /// <inheritdoc />
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        //if (task.Status == TaskStatus.Created)
        //{
        //    if (taskWasPreviouslyQueued)
        //        TryDequeue(task);
        //    return TryExecuteTask(task);
        //}
        return false;
    }
    /// <inheritdoc />
    protected override bool TryDequeue(Task task)
        => _queue.Remove(task);
    #endregion
    #region IQueueProcessor<Task>
    async void IQueueProcessor<Task>.Run(IQueue<Task> queue, ThreadJobService<Task> service, CancellationToken token)
    {
        while (queue.TryDequeue(out var item))
        {
            if (service.Activate(item))
            {
                switch(item.Status)
                {
                    case TaskStatus.Created:
                    case TaskStatus.WaitingForActivation:
                    case TaskStatus.WaitingToRun:
                        try
                        {
                            TryExecuteTask(item);
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    default:
                        try
                        {
                            await item.ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                }
            }
            if (token.IsCancellationRequested)
                break;
        }
        // 线程用完释放(回收)
        service.Dispose();
    }
    #endregion
}
