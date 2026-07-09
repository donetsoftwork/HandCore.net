namespace Hand.Text;

/// <summary>
/// 字符串转换器
/// </summary>
/// <typeparam name="TDest"></typeparam>
public abstract class StringConverter<TDest>
    : IStringConverter<TDest>
{
    /// <inheritdoc />
    public virtual TDest Convert(string source)
        => Convert(source.AsSpan());
    /// <inheritdoc />
    public virtual TDest Convert(char[] source)
         => Convert(source.AsSpan());
    /// <inheritdoc />
    public abstract TDest Convert(ReadOnlySpan<char> source);
}
