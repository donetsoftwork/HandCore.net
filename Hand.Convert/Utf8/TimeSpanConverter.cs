using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 TimeSpan转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class TimeSpanConverter(char standardFormat, TimeSpan defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, TimeSpan>
{
    #region 配置
    private readonly TimeSpan _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public TimeSpan DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public TimeSpan Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out TimeSpan result) ? result : _defaultValue;
}
