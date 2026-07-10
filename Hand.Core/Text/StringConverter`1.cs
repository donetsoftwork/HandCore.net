using Hand.Maping;

namespace Hand.Text;

/// <summary>
/// 文本转换器
/// </summary>
/// <typeparam name="TDest"></typeparam>
public abstract class StringConverter<TDest>
    : IConverter<string, TDest>
    , IConverter<char[], TDest>
    , ISpanConverter<char, TDest>
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
