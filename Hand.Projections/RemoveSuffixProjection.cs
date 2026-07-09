using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 后缀去除投影
/// </summary>
/// <param name="suffix">后缀</param>
/// <param name="comparison"></param>
public class RemoveSuffixProjection(string suffix, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>, IProjection<string>
{
    #region 配置
    /// <summary>
    /// 后缀
    /// </summary>
    protected readonly string _suffix = suffix;
    /// <summary>
    /// 后缀长度
    /// </summary>
    protected readonly int _suffixLength = suffix.Length;
    /// <summary>
    /// 验证规则
    /// </summary>
    protected readonly IValidation<string> _validation = Logic.Suffix(suffix, comparison);
    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix
        => _suffix;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => _validation.Validate(source) && source.Length > _suffixLength;
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        => source[..^_suffixLength];
#else
        => source.Substring(0, source.Length - _suffixLength);
#endif
#if NET7_0_OR_GREATER
    /// <summary>
    /// 转化为Span
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public virtual ReadOnlySpan<char> ConvertSpan(ReadOnlySpan<char> source)
        => source[..^_suffixLength];
#endif
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="suffix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static string Convert(string source, string suffix, StringComparison comparison = StringComparison.Ordinal)
    {
        if (source.EndsWith(suffix, comparison))
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return source[..^suffix.Length];
#else
            return source.Substring(suffix.Length);
#endif
        return source;
    }
    /// <inheritdoc />
    string IConverter<string, string>.Convert(string source)
    {
        if (Validate(source))
            return Convert(source);
        return source;
    }
}
