using Hand.Collections;
using System.Collections.Concurrent;

namespace Hand.ConcurrentCollections;

/// <summary>
/// ConcurrentQueue适配器
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="target"></param>
public class ConcurrentQueueAdapter<TItem>(ConcurrentQueue<TItem> target)
    : IQueue<TItem>
{
    /// <summary>
    /// ConcurrentQueue适配器
    /// </summary>
    public ConcurrentQueueAdapter()
        : this(new())
    {
    }
    #region 配置
    private readonly ConcurrentQueue<TItem> _target = target;
    /// <summary>
    /// 被适配的对象
    /// </summary>
    public ConcurrentQueue<TItem> Target
        => _target;
    /// <summary>
    /// 元素数量
    /// </summary>
    public int Count
        => _target.Count;
    /// <summary>
    /// 是否为空
    /// </summary>
    public bool IsEmpty
        => _target.IsEmpty;
    #endregion
    #region IQueue<TItem>
    /// <inheritdoc />
    public void Enqueue(TItem item)
        => _target.Enqueue(item);
    /// <inheritdoc />
    public bool TryDequeue(out TItem item)
        => _target.TryDequeue(out item);
    #endregion
    /// <summary>
    /// 尝试获取
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryPeek(out TItem item)
        => _target.TryPeek(out item);
}
