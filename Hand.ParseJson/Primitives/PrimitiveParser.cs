using Hand.ParseJson.Cachers;
using Hand.ParseJson.Nodes;
using Hand.Utf8;
using System.Text.Json;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 基础类型解析器
/// </summary>
public class PrimitiveParser(object defaultValue)
    : ValueReader<object>(defaultValue)
{
    /// <summary>
    /// 解析器
    /// </summary>
    protected static readonly Parser _parser = PrimitiveReaderCacher.Parser;

    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out object result)
    {
        if (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (str is not null)
                    {
                        result = str;
                        return true;
                    }
                    break;
                case JsonTokenType.True:
                    result = true;
                    return true;
                case JsonTokenType.False:
                    result = false;
                    return true;
                case JsonTokenType.Number:
                    if(_parser.TryParse(GetOriginalValue(ref reader), out double number))
                    {
                        result = number;
                        return true;
                    }
                    break;
                case JsonTokenType.Null:
                case JsonTokenType.StartObject:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    break;
                default:
                    result = StringConverter.GetString(GetOriginalValue(ref reader));
                    return true;
            }
        }
        result = _defaultValue;
        return false;

    }
    /// <inheritdoc />
    protected override bool TryParse(ReadOnlySpan<byte> bytes, out object result)
    {
        result = _defaultValue;
        return false;
    }
}
