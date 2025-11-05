namespace Hand.Collections;

/// <summary>
/// 堆栈接口
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IStack<TItem>
{
    /// <summary>
    /// 是否为空
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// 压入(放最上面)
    /// </summary>
    /// <param name="item"></param>
    void Push(TItem item);
    /// <summary>
    /// 弹出(最上面的)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool TryPop(out TItem item);
}
