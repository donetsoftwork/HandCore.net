using System.Collections.Generic;

namespace Hand.Maping.Recognizers;

/// <summary>
/// 交叉投影识别器
/// </summary>
/// <param name="projection">投影规则</param>
/// <param name="comparer"></param>
public sealed class CrossRecognizer<TKey>(IProjection<TKey> projection, IEqualityComparer<TKey> comparer)
    : IRecognizer<TKey>
    where TKey : notnull
{
    #region 配置
    private readonly IProjection<TKey> _projection = projection;
    private readonly IEqualityComparer<TKey> _comparer = comparer;

    /// <summary>
    /// 投影规则
    /// </summary>
    public IProjection<TKey> Projection
        => _projection;
    /// <summary>
    /// 比较器
    /// </summary>
    public IEqualityComparer<TKey> Comparer
        => _comparer;
    #endregion
    /// <inheritdoc />
    IDictionary<TKey, TValue> IRecognizer<TKey>.Recognize<TValue>(IDictionary<TKey, TValue> source)
        => _projection.Cross(source, _comparer);
}
