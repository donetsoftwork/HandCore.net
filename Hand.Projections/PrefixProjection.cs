using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 增加前缀投影
/// </summary>
/// <param name="prefix">前缀</param>
/// <param name="comparison"></param>
public class PrefixProjection(string prefix, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>, IConverter<string, string>, IValidation<string>
{
    #region 配置
    private readonly string _prefix = prefix;
    /// <summary>
    /// 前缀
    /// </summary>
    public string Prefix
        => _prefix;
    /// <summary>
    /// 验证规则
    /// </summary>
    private readonly IValidation<string> _validation = Logic.Prefix(prefix, comparison);
    #endregion
    #region IProjection<string>
    /// <inheritdoc />
    public override bool TryConvert(string source, out string value)
    {
        if (_validation.Validate(source))
        {
            value = source;
            return false;
        }
        value = _prefix + source;
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
        => _prefix + source;
    #endregion
}
