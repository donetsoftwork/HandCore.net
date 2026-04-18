using System;

namespace Hand.Rule;

/// <summary>
/// 包含规则
/// </summary>
/// <param name="fragment"></param>
/// <param name="start"></param>
/// <param name="comparison"></param>
public class IncludeRule(string fragment, int start, StringComparison comparison = StringComparison.Ordinal)
    : IValidation<string>
{
    #region 配置
    private readonly string _fragment = fragment;
    private readonly StringComparison _comparison = comparison;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public bool Validate(string argument)
        => argument.IndexOf(_fragment, start, _comparison) >= start;
    #endregion
}
