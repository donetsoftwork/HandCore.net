using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 属性解析
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="name"></param>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class PropertyParser<TValue>(string name, IJsonParser<TValue> original, TValue defaultValue)
    : ValueReader<TValue>(defaultValue)
{
    #region 配置
    private readonly string _name = name;
    private readonly IJsonParser<TValue> _original = original;
    /// <summary>
    /// 属性名
    /// </summary>
    public string Name 
        => _name;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IJsonParser<TValue> Original 
        => _original;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out TValue result)
    {
        if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(_name))
            return _original.TryParse(ref reader, out result);
        result = _defaultValue;
        return false;
    }
    /// <inheritdoc />
    protected override bool TryParse(ReadOnlySpan<byte> bytes, out TValue result)
    {
        // 只实现基类,并不实际调用
        result = _defaultValue;
        return false;
    }
}
