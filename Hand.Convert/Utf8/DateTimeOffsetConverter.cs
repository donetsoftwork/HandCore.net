using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 DateTimeOffset转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class DateTimeOffsetConverter(char standardFormat, DateTimeOffset defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, DateTimeOffset>
{
    #region 配置
    private readonly DateTimeOffset _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public DateTimeOffset DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public DateTimeOffset Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out DateTimeOffset result) ? result : _defaultValue;
}
