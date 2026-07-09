using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 int转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class IntConverter(char standardFormat, int defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, int>
{
    #region 配置
    private readonly int _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public int DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public int Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out int result) ? result : _defaultValue;
}
