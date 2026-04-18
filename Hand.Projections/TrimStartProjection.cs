using Hand.Rule;

namespace Hand.Maping;

/// <summary>
/// 前导字符去除投影
/// </summary>
public class TrimStartProjection(char[] starts)
    : ProjectionBase<string>, IConverter<string, string>
{
    #region 配置
    /// <summary>
    ///  前导字符
    /// </summary>
    private readonly char[] _starts = starts;
    /// <summary>
    /// 验证规则
    /// </summary>
    protected readonly IValidation<char> _validation = Logic.Included(starts);
    /// <summary>
    ///  前导字符
    /// </summary>
    public char[] Starts
        => _starts;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => source.Length > 0 && _validation.Validate(source[0]);
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
        => source.TrimStart(_starts);
}
