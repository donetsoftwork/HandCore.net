using Hand.Convert;
using Hand.ParseJson.Nodes;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 基础类型读取器
/// </summary>
/// <param name="parser"></param>
/// <param name="defaultValue"></param>
public class PrimitiveReader<TValue>(ISpanParser<byte, TValue> parser, TValue defaultValue)
    : ValueReader<TValue>(defaultValue)
{
    #region 配置
    /// <summary>
    /// ReadOnlySpan转化器
    /// </summary>
    private readonly ISpanParser<byte, TValue> _parser = parser;
    /// <summary>
    /// ReadOnlySpan转化器
    /// </summary>
    public ISpanParser<byte, TValue> Converter
        => _parser;
    #endregion
    /// <inheritdoc />
    protected override bool TryParser(ReadOnlySpan<byte> bytes, out TValue result)
        => _parser.TryParse(bytes, out result);
    ///// <inheritdoc />
    //protected override TValue GetValue(ref Utf8JsonReader reader)
    //    => _converter.TryParse(GetOriginalValue(ref reader));
}