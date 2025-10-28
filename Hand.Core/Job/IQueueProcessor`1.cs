namespace Hand.Job;

/// <summary>
/// 排队处理器
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IQueueProcessor<TItem>
    : IProcessor<TItem>
{
    /// <summary>
    /// 尝试获取子项
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool TryTake(out TItem item);
}
