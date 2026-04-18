using Hand.Rule;
using System.Linq;

namespace Hand.Maping;

/// <summary>
/// 前后字符去除投影
/// </summary>
public class TrimProjection(char[] trimChars)
    : ProjectionBase<string>, IConverter<string, string>
{
    #region 配置
    /// <summary>
    ///  字符
    /// </summary>
    private readonly char[] _trimChars = trimChars;
    /// <summary>
    /// 验证规则
    /// </summary>
    protected readonly IValidation<char> _validation = Logic.Included(trimChars);
    /// <summary>
    ///  字符
    /// </summary>
    public char[] TrimChars
        => _trimChars;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => source.Length > 0 && (_validation.Validate(source[0]) || _validation.Validate(source.Last()));
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
        => source.Trim(_trimChars);
}
