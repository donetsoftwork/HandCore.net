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
    ///// <inheritdoc />
    //protected override string GetValue(ref Utf8JsonReader reader)
    //{
    //    return reader.TokenType switch
    //    {
    //        JsonTokenType.String => reader.GetString() ?? _defaultValue,
    //        JsonTokenType.True => "true",
    //        JsonTokenType.False => "false",
    //        JsonTokenType.Null => _defaultValue,
    //        _ => base.GetValue(ref reader)
    //    };
    //}
    /// <inheritdoc />
    public override bool TryParser(ref Utf8JsonReader reader, out string result)
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
    protected override bool TryParser(ReadOnlySpan<byte> bytes, out string result)
    {
        result = StringConverter.GetString(bytes); 
        return true;
    }
}
