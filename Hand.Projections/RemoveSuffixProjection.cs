using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 后缀去除投影
/// </summary>
/// <param name="suffix">后缀</param>
/// <param name="comparison"></param>
public class RemoveSuffixProjection(string suffix, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>
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
        => source.Substring(0, source.Length - _suffixLength);
}
