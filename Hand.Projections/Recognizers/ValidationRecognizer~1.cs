using Hand.Rule;
using System.Collections.Generic;

namespace Hand.Maping.Recognizers;

/// <summary>
/// 验证成员识别器
/// </summary>
/// <param name="keys">投影规则</param>
/// <param name="exclude"></param>
/// <param name="comparer"></param>
public sealed class ValidationRecognizer<TKey>(IEnumerable<TKey> keys, bool exclude, IEqualityComparer<TKey> comparer)
    : IRecognizer<TKey>
    where TKey : notnull
{
    #region 配置
    //private readonly IEnumerable<TKey> _keys = keys;
    //private readonly bool _exclude = exclude;
    private readonly IValidation<TKey> _validation = exclude ? Logic.Included(comparer, keys).Not() : Logic.Included(comparer, keys);
    private readonly IEqualityComparer<TKey> _comparer = comparer;
    /// <summary>
    /// 验证规则
    /// </summary>
    public IValidation<TKey> Validation
        => _validation;
    /// <summary>
    /// 比较器
    /// </summary>
    public IEqualityComparer<TKey> Comparer
        => _comparer;
    #endregion
    /// <inheritdoc />
    IDictionary<TKey, TValue> IRecognizer<TKey>.Recognize<TValue>(IDictionary<TKey, TValue> source)
        => _validation.Filter(source, _comparer);
    /// <inheritdoc />
    IRecognizer<TKey> IRecognizer<TKey>.Reverse()
        => this;
}
