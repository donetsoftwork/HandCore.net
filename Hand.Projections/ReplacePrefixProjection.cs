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
    private readonly int _replacementLen = replacement.Length;
    /// <summary>
    /// 替身
    /// </summary>
    public string Replacement
        => _replacement;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => _validation.Validate(source) && (_replacementLen > 0 || source.Length > _prefixLength);
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
#if NET7_0_OR_GREATER
        => string.Concat(_replacement, ConvertSpan(source));
#else
        => _replacement + base.Convert(source);
#endif
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="prefix"></param>
    /// <param name="replacement"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static string Convert(string source, string prefix, string replacement, StringComparison comparison = StringComparison.Ordinal)
    {
        if (source.StartsWith(prefix, comparison))
#if NET7_0_OR_GREATER
            return string.Concat(replacement, source.AsSpan(prefix.Length));
#elif NETSTANDARD2_1
            return string.Concat(replacement, source[prefix.Length..]);
#else
            return string.Concat(replacement, source.Substring(prefix.Length));
#endif
        return source;
    }
}
