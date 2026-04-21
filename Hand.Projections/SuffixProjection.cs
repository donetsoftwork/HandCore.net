using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 增加后缀投影
/// </summary>
/// <param name="suffix">后缀</param>
/// <param name="comparison"></param>
public class SuffixProjection(string suffix, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>, IProjection<string>
{
    #region 配置
    private readonly string _suffix = suffix;
    /// <summary>
    /// 验证规则
    /// </summary>
    private readonly IValidation<string> _validation = Logic.Suffix(suffix, comparison);
    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix
        => _suffix;
    /// <inheritdoc />
    public override bool TryConvert(string source, out string value)
    {
        if (_validation.Validate(source))
        {
            value = source;
            return false;
        }
        value = source + _suffix;
        return true;
    }
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => _validation.Not(source);
    #endregion
    #region IConverter<string, string>
    /// <inheritdoc />
    public override string Convert(string source)
        => source + _suffix;
    #endregion
}
