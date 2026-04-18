using System;

namespace Hand.Rule;

/// <summary>
/// 前缀规则
/// </summary>
/// <param name="prefix"></param>
/// <param name="comparison"></param>
public class PrefixRule(string prefix, StringComparison comparison = StringComparison.Ordinal)
    : IValidation<string>
{
    #region 配置
    private readonly string _prefix = prefix;
    private readonly StringComparison _comparison = comparison;

    /// <summary>
    /// 前缀
    /// </summary>
    public string Prefix
        => _prefix;
    /// <summary>
    /// 字符串比较类别
    /// </summary>
    public StringComparison Comparison 
        => _comparison;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public bool Validate(string argument)
        => argument.StartsWith(_prefix, _comparison);
    #endregion
}
