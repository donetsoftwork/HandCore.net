using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 long转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class DateTimeConverter(char standardFormat, DateTime defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, DateTime>
{
    #region 配置
    private readonly DateTime _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public DateTime DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public DateTime Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out DateTime result) ? result : _defaultValue;
}
