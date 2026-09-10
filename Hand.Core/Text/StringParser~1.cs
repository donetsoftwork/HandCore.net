using Hand.Convert;
using System.Diagnostics.CodeAnalysis;

namespace Hand.Text;

/// <summary>
/// 文本解析
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class StringParser<TResult>
    : IParser<string, TResult>
    , IParser<char[], TResult>
    , ISpanParser<char, TResult>
{
    /// <inheritdoc />
    public virtual bool TryParse(string resource, [NotNullWhen(true)] out TResult result)
        => TryParse(resource.AsSpan(), out result);
    /// <inheritdoc />
    public virtual bool TryParse(char[] resource, [NotNullWhen(true)] out TResult result)
        => TryParse(resource.AsSpan(), out result);
    /// <inheritdoc />
    public abstract bool TryParse(ReadOnlySpan<char> resource, out TResult result);
}
