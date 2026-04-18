using System.Collections.Generic;

namespace Hand.Maping.Complexs;

/// <summary>
/// 链表投影(责任链模式)
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="chain"></param>
public class ChainProjection<T>(LinkedList<IProjection<T>> chain)
    : IProjection<T>
{
    /// <summary>
    /// 链表映射
    /// </summary>
    /// <param name="projections"></param>
    public ChainProjection(params IProjection<T>[] projections)
        : this(new LinkedList<IProjection<T>>(projections))
    {
    }
    #region 配置
    /// <summary>
    /// 链表
    /// </summary>
    protected readonly LinkedList<IProjection<T>> _chain = chain;
    /// <summary>
    /// 添加投影
    /// </summary>
    /// <param name="projection"></param>
    public void AddLast(IProjection<T> projection)
        => _chain.AddLast(projection);
    /// <summary>
    /// 插入投影
    /// </summary>
    /// <param name="projection"></param>
    public void AddFirst(IProjection<T> projection)
        => _chain.AddFirst(projection);
    #endregion
    /// <summary>
    /// 尝试转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool TryConvert(T source, out T value)
    {
        foreach (var item in _chain)
        {
            if (item.TryConvert(source, out value))
                return true;
        }
        value = source;
        return false;
    }
}
