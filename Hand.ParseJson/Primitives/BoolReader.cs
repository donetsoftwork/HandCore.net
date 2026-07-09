using System.Text.Json;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 布尔值读取器
/// </summary>
/// <param name="defaultValue"></param>
public sealed class BoolReader(bool defaultValue)
    : BoolReader<bool>(true, false, defaultValue)
{
    /// <inheritdoc />
    public override bool GetDefaultValue(ref Utf8JsonReader reader)
        => _converter.Convert(GetOriginalValue(ref reader));
}
