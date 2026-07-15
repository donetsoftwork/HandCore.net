using Hand.ParseJson.Cachers;
using Hand.ParseJson.Nodes;
using Hand.Utf8;
using System.Text.Json;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 布尔值读取器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="trueValue"></param>
/// <param name="falseValue"></param>
/// <param name="defaultValue"></param>
public class BoolReader<TValue>(TValue trueValue, TValue falseValue, TValue defaultValue)
    : ValueReader<TValue>(defaultValue)
{
    #region 配置

    private readonly TValue _trueValue = trueValue;
    private readonly TValue _falseValue = falseValue;
    /// <summary>
    /// 解析器
    /// </summary>
    protected static readonly Parser _parser = PrimitiveReaderCacher.Parser;

    /// <summary>
    /// true值
    /// </summary>
    public TValue TrueValue => _trueValue;
    /// <summary>
    /// false值
    /// </summary>
    public TValue FalseValue => _falseValue;
    /// <summary>
    /// 解析器
    /// </summary>
    public Parser Parser
        => _parser;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out TValue result)
    {
        if (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    result = _trueValue;
                    return true;
                case JsonTokenType.False:
                    result = _falseValue;
                    return true;
                case JsonTokenType.Null:
                case JsonTokenType.StartObject:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    result = _defaultValue;
                    return false;
            }
            if (TryParse(GetOriginalValue(ref reader), out result))
                return true;
        }
        result = _defaultValue;
        return false;
    }
    /// <inheritdoc />
    protected override bool TryParse(ReadOnlySpan<byte> bytes, out TValue result)
    {
        if(_parser.TryParse(bytes, out bool state))
        {
            result = state ? _trueValue : _falseValue;
            return true;
        }
        result = _defaultValue;
        return false;
    }
}
