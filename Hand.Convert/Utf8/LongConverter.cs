using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 long转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class LongConverter(char standardFormat, long defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, long>
{
    #region 配置
    private readonly long _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public long DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public long Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out long result) ? result : _defaultValue;
}
