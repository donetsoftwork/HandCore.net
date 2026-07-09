using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 float转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class FloatConverter(char standardFormat, float defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, float>
{
    #region 配置
    private readonly float _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public float DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public float Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out float result) ? result : _defaultValue;
}
