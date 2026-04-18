using Hand.Naming;
using Hand.Rule;

namespace Hand.Maping;

/// <summary>
/// 命名规则投影
/// </summary>
/// <param name="validation"></param>
/// <param name="converter"></param>
public sealed class NamingProjection(IValidation<string> validation, INameConverter converter)
    : IProjection<string>, IValidation<string>, IConverter<string, string>
{
    #region 配置
    /// <summary>
    /// 验证器
    /// </summary>
    private readonly IValidation<string> _validation = validation;
    /// <summary>
    /// 转化器
    /// </summary>
    private readonly INameConverter _converter = converter;
    #endregion
    #region IProjection<string>
    /// <inheritdoc />
    public bool TryConvert(string source, out string value)
    {
        if (_validation.Validate(source))
        {
            value = _converter.Convert(source);
            return true;
        }
        value = source;
        return false;
    }
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public bool Validate(string argument)
        => _validation.Validate(argument);
    #endregion
    #region IConverter<string, string>
    /// <inheritdoc />
    public string Convert(string source)
        => _converter.Convert(source);
    #endregion
}
