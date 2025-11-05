using Hand.Collections;

namespace Hand.Job;

/// <summary>
/// Action处理器
/// </summary>
public sealed class ActionProcessor : IQueueProcessor<Action>
{
    /// <inheritdoc />
    public void Run(IQueue<Action> queue, ThreadJobService<Action> service, CancellationToken token)
    {
        while (queue.TryDequeue(out var item))
        {
            if (service.Activate(item))
            {
                try
                {
                    item();
                }
                catch { }
            }
            if (token.IsCancellationRequested)
                break;
        }
        // 线程用完释放(回收)
        service.Dispose();
    }
    /// <summary>
    /// 默认实例
    /// </summary>
    public static readonly ActionProcessor Instance = new();
}
