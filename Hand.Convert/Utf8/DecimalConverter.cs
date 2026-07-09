using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 decimal转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class DecimalConverter(char standardFormat, decimal defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, decimal>
{
    #region 配置
    private readonly decimal _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public decimal DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public decimal Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out decimal result) ? result : _defaultValue;
}
