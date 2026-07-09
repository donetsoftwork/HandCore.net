using Hand.Maping;

namespace Hand.Bytes;

/// <summary>
/// 二进制转换器
/// </summary>
public interface IBinaryConverter<out TDest>
    : IConverter<byte[], TDest>, ISpanConverter<byte, TDest>
{
}
