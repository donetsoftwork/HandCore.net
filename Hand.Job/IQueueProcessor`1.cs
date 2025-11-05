using Hand.Collections;
using Hand.States;

namespace Hand.Job;

/// <summary>
/// 排队处理器
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IQueueProcessor<TItem>
{
    /// <summary>
    /// 执行队列
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="service"></param>
    /// <param name="token"></param>
    void Run(IQueue<TItem> queue, ThreadJobService<TItem> service, CancellationToken token);
}
