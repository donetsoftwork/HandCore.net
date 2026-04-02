namespace Hand.Converters;

/// <summary>
/// 默认命名转化
/// </summary>
public class DefaultNameConverter : INameConverter
{
    private DefaultNameConverter() { }
    /// <inheritdoc />
    public string Convert(string name, int startIndex = 0)
        => string.IsNullOrEmpty(name) ? string.Empty : name.Substring(startIndex);
    /// <inheritdoc />
    public string Convert(ReadOnlySpan<char> name)
        => name.ToString();
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly INameConverter Instance = new DefaultNameConverter();
}
