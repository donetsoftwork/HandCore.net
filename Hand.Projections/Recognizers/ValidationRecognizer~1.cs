using Hand.Rule;
using System.Collections.Generic;

namespace Hand.Maping.Recognizers;

/// <summary>
/// 验证识别器
/// </summary>
/// <param name="validation">投影规则</param>
/// <param name="comparer"></param>
public sealed class ValidationRecognizer<TKey>(IValidation<TKey> validation, IEqualityComparer<TKey> comparer)
    : IRecognizer<TKey>
    where TKey : notnull
{
    #region 配置
    private readonly IValidation<TKey> _validation = validation;
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
}
