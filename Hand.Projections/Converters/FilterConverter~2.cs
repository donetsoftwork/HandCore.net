using System.Collections.Generic;

namespace Hand.Maping.Converters;

/// <summary>
/// 过滤转化器
/// </summary>
/// <param name="projection">投影规则</param>
public class FilterConverter<TKey, TValue>(IProjection<TKey> projection)
    : IConverter<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>>
    where TKey : notnull
{
    #region 配置
    private readonly IProjection<TKey> _projection = projection;
    /// <summary>
    /// 投影规则
    /// </summary>
    public IProjection<TKey> Projections
        => _projection;
    #endregion
    /// <inheritdoc />
    IDictionary<TKey, TValue> IConverter<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>>.Convert(IDictionary<TKey, TValue> source)
        => _projection.Filter(source);
}
