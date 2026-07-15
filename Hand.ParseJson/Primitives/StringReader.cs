using Hand.ParseJson.Nodes;
using System.Text.Json;
using Hand.Utf8;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 字符串读取器
/// </summary>
/// <param name="defaultValue"></param>
public class StringReader(string defaultValue)
    : ValueReader<string>(defaultValue)
{
    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out string result)
    {
        if (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (str is null)
                    {
                        result = _defaultValue;
                        return false;
                    }
                    result = str;
                    return true;
                case JsonTokenType.True:
                    result = "true";
                    return true;
                case JsonTokenType.False:
                    result = "false";
                    return true;
                case JsonTokenType.Null:
                case JsonTokenType.StartObject:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    result = _defaultValue;
                    return false;
                default:
                    result = StringConverter.GetString(GetOriginalValue(ref reader));
                    return true;
            }
        }
        result = _defaultValue;
        return false;
    }
    /// <inheritdoc />
    protected override bool TryParse(ReadOnlySpan<byte> bytes, out string result)
    {
        result = StringConverter.GetString(bytes); 
        return true;
    }
}
