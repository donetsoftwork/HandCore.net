using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 double转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class DoubleConverter(char standardFormat, double defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, double>
{
    #region 配置
    private readonly double _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public double DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public double Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out double result) ? result : _defaultValue;
}
