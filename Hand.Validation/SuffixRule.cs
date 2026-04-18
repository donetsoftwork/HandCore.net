using System;

namespace Hand.Rule;

/// <summary>
/// 后缀规则
/// </summary>
/// <param name="suffix"></param>
/// <param name="comparison"></param>
public class SuffixRule(string suffix, StringComparison comparison = StringComparison.Ordinal)
    : IValidation<string>
{
    #region 配置
    private readonly string _suffix = suffix;
    private readonly StringComparison _comparison = comparison;

    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix
        => _suffix;
    /// <summary>
    /// 字符串比较类别
    /// </summary>
    public StringComparison Comparison
        => _comparison;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public bool Validate(string argument)
        => argument.EndsWith(_suffix, _comparison);
    #endregion
}
