using Hand.Collections;
using System.Collections.Concurrent;

namespace Hand.ConcurrentCollections;

/// <summary>
/// ConcurrentStack适配器
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="target"></param>
public class ConcurrentStackAdapter<TItem>(ConcurrentStack<TItem> target)
    : IStack<TItem>
{
    #region 配置
    private readonly ConcurrentStack<TItem> _target = target;
    /// <summary>
    /// 被适配的对象
    /// </summary>
    public ConcurrentStack<TItem> Target
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
    #region IStack<TItem>
    /// <inheritdoc />
    public void Push(TItem item)
        => _target.Push(item);
    /// <inheritdoc />
    public bool TryPop(out TItem item)
        => TryPop(out item);
    #endregion
    /// <summary>
    /// 尝试获取
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryPeek(out TItem item)
        => _target.TryPeek(out item);
}
