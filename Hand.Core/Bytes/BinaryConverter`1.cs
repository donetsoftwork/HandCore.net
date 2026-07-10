using Hand.Maping;

namespace Hand.Bytes;

/// <summary>
/// 二进制转换器
/// </summary>
/// <typeparam name="TDest"></typeparam>
public abstract class BinaryConverter<TDest>
    : IConverter<byte[], TDest>
    , ISpanConverter<byte, TDest>
{
    /// <inheritdoc />
    public virtual TDest Convert(byte[] source)
        => Convert(source.AsSpan());
    /// <inheritdoc />
    public abstract TDest Convert(ReadOnlySpan<byte> source);
}
