using Hand.Utf8;
using System.Text.Json;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 字符串读取器
/// </summary>
/// <param name="defaultValue"></param>
public class StringReader(string defaultValue)
    : PrimitiveReader<string>(StringConverter.Instance, defaultValue)
{
    /// <inheritdoc />
    protected override string GetValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString() ?? _defaultValue,
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            JsonTokenType.Null => _defaultValue,
            _ => base.GetValue(ref reader)
        };
    }
}
