namespace Hand.Collections;

/// <summary>
/// 队列接口
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IQueue<TItem>
{
    /// <summary>
    /// 是否为空
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// 入队(排最后)
    /// </summary>
    /// <param name="item"></param>
    void Enqueue(TItem item);
    /// <summary>
    /// 出队(最前面)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool TryDequeue(out TItem item);
}
