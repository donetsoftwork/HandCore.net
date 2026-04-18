namespace Hand.Rule;

/// <summary>
/// 包含任一字符
/// </summary>
/// <param name="start"></param>
/// <param name="parts"></param>
public class IncludeAnyRule(int start, params char[] parts)
    : IValidation<string>
{
    #region 配置
    private readonly int _start = start;
    private readonly char[] _parts = parts;
    /// <summary>
    /// 查找的起始位置
    /// </summary>
    public int Start
        => _start;
    /// <summary>
    /// 被查找的字符
    /// </summary>
    public char[] Parts
        => _parts;
    #endregion

    #region IValidation<string>
    /// <inheritdoc />
    public bool Validate(string argument)
        => argument.IndexOfAny(_parts, _start) >= _start;
    #endregion
}
