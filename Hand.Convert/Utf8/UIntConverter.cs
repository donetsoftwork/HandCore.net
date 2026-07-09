using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 uint转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class UIntConverter(char standardFormat, uint defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, uint>
{
    #region 配置
    private readonly uint _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public uint DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public uint Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out uint result) ? result : _defaultValue;
}
