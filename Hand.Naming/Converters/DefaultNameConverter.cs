using Hand.Text;

namespace Hand.Converters;

/// <summary>
/// 默认命名转化
/// </summary>
public class DefaultNameConverter : StringConverter<string>
{
    private DefaultNameConverter() { }
    /// <inheritdoc />
    public override string Convert(string name)
        => name;
    /// <inheritdoc />
    public override string Convert(char[] source)
        => new(source);
    /// <inheritdoc />
    public override string Convert(ReadOnlySpan<char> name)
        => name.ToString();
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IStringConverter<string> Instance = new DefaultNameConverter();
}
