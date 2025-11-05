using Hand.Collections;

namespace Hand.ConcurrentCollections;

/// <summary>
/// LinkedList适配器队列
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="target"></param>
public class LinkedListAdaptQueue<TItem>(LinkedList<TItem> target)
    : IQueue<TItem>
{
    /// <summary>
    /// LinkedList适配器队列
    /// </summary>
    public LinkedListAdaptQueue()
        : this(new LinkedList<TItem>())
    {
    }
    #region 配置
    private readonly LinkedList<TItem> _target = target;
#if NET9_0_OR_GREATER
    private readonly Lock _targetLock = new();
#endif
    /// <summary>
    /// 被适配的对象
    /// </summary>
    public LinkedList<TItem> Target
        => _target;
    /// <summary>
    /// 子元素
    /// </summary>
    public IEnumerable<TItem> Items
    {
        get
        {
#if NET9_0_OR_GREATER
            lock (_targetLock)
#else
            lock (_target)
#endif
            {
                return [.. _target];
            }
        }
    }
    /// <summary>
    /// 元素数量
    /// </summary>
    public int Count
        => _target.Count;
    /// <inheritdoc />
    public bool IsEmpty
        => _target.Count == 0;
    #endregion
    #region IQueue<TItem>
    /// <inheritdoc />
    public void Enqueue(TItem item)
    {
#if NET9_0_OR_GREATER
        lock (_targetLock)
#else
        lock (_target)
#endif
        {
            _target.AddLast(item);
        }
    }
    /// <inheritdoc />
    public bool TryDequeue(out TItem item)
    {
        if (_target.Count > 0)
        {
            var state = false;
            LinkedListNode<TItem> first = null;
#if NET9_0_OR_GREATER
            lock (_targetLock)
#else
            lock (_target)
#endif
            {
                first = _target.First;
                if (first is not null)
                {
                    state = true;
                    _target.RemoveFirst();
                }
            }
            if (state)
            {
                item = first.Value;
                return true;
            }
        }
        item = default;
        return false;
    }
    #endregion
    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(TItem item)
    {
#if NET9_0_OR_GREATER
        lock (_targetLock)
#else
        lock (_target)
#endif
        {
            return _target.Remove(item);
        }
    }
}
