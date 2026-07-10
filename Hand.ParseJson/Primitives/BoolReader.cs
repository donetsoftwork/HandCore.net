namespace Hand.ParseJson.Primitives;

/// <summary>
/// 布尔值读取器
/// </summary>
/// <param name="defaultValue"></param>
public sealed class BoolReader(bool defaultValue)
    : BoolReader<bool>(true, false, defaultValue)
{
    ///// <inheritdoc />
    //public override bool GetDefaultValue(ref Utf8JsonReader reader)
    //    => _parser.Convert(GetOriginalValue(ref reader));
    /// <inheritdoc />
    protected override bool TryParser(ReadOnlySpan<byte> bytes, out bool result)
    {
        if (_parser.TryParse(bytes, out result))
            return true;
        result = _defaultValue;
        return false;
    }
}
