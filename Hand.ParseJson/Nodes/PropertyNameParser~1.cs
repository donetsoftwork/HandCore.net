using Hand.Convert;
using Hand.ParseJson.Primitives;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 属性名读取器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="parser"></param>
/// <param name="defaultValue"></param>
public class PropertyNameParser<TValue>(ISpanParser<byte, TValue> parser, TValue defaultValue)
    : PrimitiveReader<TValue>(parser, defaultValue)
{
    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out TValue result)
    {
        if (reader.TokenType == JsonTokenType.PropertyName && _parser.TryParse(GetOriginalValue(ref reader), out result))
            return true;
        result = _defaultValue;
        return false;
    }
}
