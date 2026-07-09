using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 byte转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class ByteConverter(char standardFormat, byte defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, byte>
{
    #region 配置
    private readonly byte _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public byte DefaultValue 
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public byte Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out byte result) ? result : _defaultValue;
}
