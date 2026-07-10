using Hand.Convert;

namespace Hand.Bytes;

/// <summary>
/// 二进制解析器
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class BinaryParser<TResult>
    : IParser<byte[], TResult>
    , ISpanParser<byte, TResult>
{
    /// <inheritdoc />
    public virtual bool TryParse(byte[] resource, out TResult result)
        => TryParse(resource.AsSpan(), out result);
    /// <inheritdoc />
    public abstract bool TryParse(ReadOnlySpan<byte> resource, out TResult result);
}
