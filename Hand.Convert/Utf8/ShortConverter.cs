using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 short转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class ShortConverter(char standardFormat, short defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, short>
{
    #region 配置
    private readonly short _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public short DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public short Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out short result) ? result : _defaultValue;
}
