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
    /// 转化器
    /// </summary>
    protected readonly BoolConverter _converter = new(default, trueValue is not null && trueValue.Equals(defaultValue));

    /// <summary>
    /// true值
    /// </summary>
    public TValue TrueValue => _trueValue;
    /// <summary>
    /// false值
    /// </summary>
    public TValue FalseValue => _falseValue;
    /// <summary>
    /// 转化器
    /// </summary>
    public BoolConverter Converter 
        => _converter;
    #endregion

    /// <inheritdoc />
    protected override TValue GetValue(ref Utf8JsonReader reader)
        => reader.TokenType switch
        {
            JsonTokenType.True => _trueValue,
            JsonTokenType.False => _falseValue,
            JsonTokenType.Null => _defaultValue,
            _ => GetDefaultValue(ref reader)
        };

    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public virtual TValue GetDefaultValue(ref Utf8JsonReader reader)
        => _converter.Convert(GetOriginalValue(ref reader)) ? _trueValue : _falseValue;
}
