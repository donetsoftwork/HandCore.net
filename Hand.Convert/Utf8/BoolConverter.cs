using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 bool转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class BoolConverter(char standardFormat, bool defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, bool>
{
    #region 配置
    private readonly bool _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public bool DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public bool Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out bool result) ? result : _defaultValue;
}
