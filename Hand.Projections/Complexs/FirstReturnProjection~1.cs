using System.Collections.Generic;

namespace Hand.Maping.Complexs;

/// <summary>
/// 快速结束投影
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="chain"></param>
public sealed class FirstReturnProjection<T>(LinkedList<IProjection<T>> chain)
    : ChainProjection<T>(chain)
{
    /// <summary>
    /// 快速结束投影
    /// </summary>
    /// <param name="projections"></param>
    public FirstReturnProjection(params IProjection<T>[] projections)
        : this(new LinkedList<IProjection<T>>(projections))
    {
    }
    #region IProjection<T>
    /// <inheritdoc />
    public override bool TryConvert(T source, out T value)
    {
        var state = false;
        foreach (var item in _chain)
        {
            if (item.TryConvert(source, out source))
            {
                state = true;
                break;
            }               
            value = source;
        }
        value = source;
        return state;
    }
    #endregion
}
