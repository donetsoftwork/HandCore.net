using Hand.Maping;

namespace Hand.Utf8;

/// <summary>
/// 字符串转化器
/// </summary>
/// <param name="defaultValue"></param>
public class CharConverter(char defaultValue)
    : ISpanConverter<byte, char>
{
    #region 配置
    private readonly char _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public char DefaultValue
        => _defaultValue;
    #endregion
    /// <inheritdoc />
    public char Convert(ReadOnlySpan<byte> source)
    {
        var str = StringConverter.GetString(source);
        if (string.IsNullOrEmpty(str))
            return _defaultValue;
        return str[0];
    }
}
