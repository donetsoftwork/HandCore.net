using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 前缀去除投影
/// </summary>
/// <param name="prefix">前缀</param>
/// <param name="comparison"></param>
public class RemovePrefixProjection(string prefix, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>, IProjection<string>
{
    #region 配置
    /// <summary>
    /// 前缀
    /// </summary>
    protected readonly string _prefix = prefix;
    /// <summary>
    /// 前缀长度
    /// </summary>
    protected readonly int _prefixLength = prefix.Length;
    /// <summary>
    /// 验证规则
    /// </summary>
    protected readonly IValidation<string> _validation = Logic.Prefix(prefix, comparison);
    /// <summary>
    /// 前缀
    /// </summary>
    public string Prefix
        => _prefix;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => _validation.Validate(source) && source.Length > _prefixLength;
    #endregion
#if NET7_0_OR_GREATER
    /// <summary>
    /// 转化为Span
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public virtual ReadOnlySpan<char> ConvertSpan(ReadOnlySpan<char> source)
        => source[_prefixLength..];
#endif
    /// <inheritdoc />
    public override string Convert(string source)
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        => source[_prefixLength..];
#else
        => source.Substring(_prefixLength);
#endif
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="prefix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static string Convert(string source, string prefix, StringComparison comparison = StringComparison.Ordinal)
    {
        if (source.StartsWith(prefix, comparison))
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return source[prefix.Length..];
#else
            return source.Substring(prefix.Length);
#endif
        return source;
    }
    #region IConverter<string, string>
    /// <inheritdoc />
    string IConverter<string, string>.Convert(string source)
    {
        if (Validate(source))
            return Convert(source);
        return source;
    }
    #endregion
}
