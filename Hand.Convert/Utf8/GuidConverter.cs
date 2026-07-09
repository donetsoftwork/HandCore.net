using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 Guid转换器
/// </summary>
/// <param name="standardFormat"></param>
/// <param name="defaultValue"></param>
public class GuidConverter(char standardFormat, Guid defaultValue = default)
    : Parser(standardFormat)
    , ISpanConverter<byte, Guid>
{
    #region 配置
    private readonly Guid _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public Guid DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public Guid Convert(ReadOnlySpan<byte> source)
        => TryParse(source, out Guid result) ? result : _defaultValue;
}
