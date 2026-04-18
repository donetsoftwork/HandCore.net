using System;

namespace Hand.Maping;

/// <summary>
/// 替换前缀投影
/// </summary>
/// <param name="prefix"></param>
/// <param name="replacement"></param>
/// <param name="comparison"></param>
public class ReplacePrefixProjection(string prefix, string replacement, StringComparison comparison = StringComparison.Ordinal)
    : RemovePrefixProjection(prefix, comparison)
{
    #region 配置
    private readonly string _replacement = replacement;
    private readonly int _replacementLend = replacement.Length;
    /// <summary>
    /// 替身
    /// </summary>
    public string Replacement
        => _replacement;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => source.StartsWith(_prefix) && (_replacementLend > 0 || source.Length > _prefixLength);
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
        => _replacement + base.Convert(source);
}
