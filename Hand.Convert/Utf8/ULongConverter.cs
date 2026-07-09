using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 ulong转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class ULongConverter(char standardFormat, ulong defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, ulong>
{
    #region 配置
    private readonly ulong _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public ulong DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public ulong Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out ulong result) ? result : _defaultValue;
}
