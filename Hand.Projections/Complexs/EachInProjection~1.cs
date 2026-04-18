using System.Collections.Generic;

namespace Hand.Maping.Complexs;

/// <summary>
/// 逐个投影
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="chain"></param>
/// <param name="failContinue"></param>
public sealed class EachInProjection<T>(LinkedList<IProjection<T>> chain, bool failContinue = true)
    : ChainProjection<T>(chain)
{
    /// <summary>
    /// 逐个投影
    /// </summary>
    /// <param name="failContinue"></param>
    /// <param name="projections"></param>
    public EachInProjection(bool failContinue, params IProjection<T>[] projections)
        : this(new LinkedList<IProjection<T>>(projections), failContinue)
    {
    }

    #region 配置
    private readonly bool _failContinue = failContinue;
    /// <summary>
    /// 失败是否继续执行
    /// </summary>
    public bool FailContinue
        => _failContinue;
    #endregion

    #region IProjection<T>
    /// <inheritdoc />
    public override bool TryConvert(T source, out T value)
    {
        foreach (var item in _chain)
        {
            if (item.TryConvert(source, out value))
                source = value;
            else if (_failContinue)
                continue;
            else
                return false;
        }
        value = source;
        return true;
    }
    #endregion
}
