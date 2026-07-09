namespace Hand.Bytes;

/// <summary>
/// 二进制转换器
/// </summary>
/// <typeparam name="TDest"></typeparam>
public abstract class BinaryConverter<TDest>
    : IBinaryConverter<TDest>
{
    /// <inheritdoc />
    public virtual TDest Convert(byte[] source)
        => Convert(source.AsSpan());
    /// <inheritdoc />
    public abstract TDest Convert(ReadOnlySpan<byte> source);
}
