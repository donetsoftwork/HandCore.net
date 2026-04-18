using System;

namespace Hand.Maping;

/// <summary>
/// 替换后缀投影
/// </summary>
/// <param name="suffix"></param>
/// <param name="replacement"></param>
/// <param name="comparison"></param>
public class ReplaceSuffixProjection(string suffix, string replacement, StringComparison comparison = StringComparison.Ordinal)
    : RemoveSuffixProjection(suffix, comparison)
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
        => _validation.Validate(source) && (_replacementLend > 0 || source.Length > _suffixLength);
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
        => base.Convert(source) + _replacement;
}
