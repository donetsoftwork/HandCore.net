using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 sbyte转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class SByteConverter(char standardFormat, sbyte defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, sbyte>
{
    #region 配置
    private readonly sbyte _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public sbyte DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public sbyte Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out sbyte result) ? result : _defaultValue;
}
