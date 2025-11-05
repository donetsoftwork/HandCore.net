using Hand.Collections;

namespace Hand.ConcurrentCollections;

/// <summary>
/// LinkedList适配器队列
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="target"></param>
public class LinkedListAdaptStack<TItem>(LinkedList<TItem> target)
    : IStack<TItem>
{
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
    /// 元素数量
    /// </summary>
    public int Count
        => _target.Count;
    /// <inheritdoc />
    public bool IsEmpty
        => _target.Count == 0;
    #endregion
    #region IStack<TItem>
    /// <inheritdoc />
    public void Push(TItem item)
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
    public bool TryPop(out TItem item)
    {
        if (_target.Count > 0)
        {
            var state = false;
            LinkedListNode<TItem> last = null;
#if NET9_0_OR_GREATER
            lock (_targetLock)
#else
            lock (_target)
#endif
            {
                last = _target.Last;
                if (last is not null)
                {
                    state = true;
                    _target.RemoveLast();
                }
            }
            if (state)
            {
                item = last.Value;
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
