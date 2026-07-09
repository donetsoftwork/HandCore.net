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
    #if NET7_0_OR_GREATER
        => string.Concat(ConvertSpan(source), _replacement);
#else
        => base.Convert(source) + _replacement;
#endif
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="suffix"></param>
    /// <param name="replacement"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static string Convert(string source, string suffix, string replacement, StringComparison comparison = StringComparison.Ordinal)
    {
        if(source.EndsWith(suffix, comparison))
        {
#if NET7_0_OR_GREATER
            return string.Concat(source.AsSpan()[..^suffix.Length], replacement);
#elif NETSTANDARD2_1_OR_GREATER
            return source[(source.Length - suffix.Length)..] + replacement;
#else
            return source.Substring(0, source.Length - suffix.Length) + replacement;
#endif
        }
        return source;
    }
}
