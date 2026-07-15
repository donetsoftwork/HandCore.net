using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 第一个属性读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class ObjectParser<TResult>(IJsonParser<TResult> original, TResult defaultValue)
    : ValueReader<TResult>(defaultValue)
{
    #region 配置
    private readonly IJsonParser<TResult> _original = original;
    /// <summary>
    /// 原始读取器
    /// </summary>
    public IJsonParser<TResult> Original
        => _original;
    #endregion
    /// <inheritdoc />
    public override bool TryParse(ref Utf8JsonReader reader, out TResult result)
    {
        if(reader.TokenType == JsonTokenType.StartObject
            && _original.TryParse(ref reader, out result))
            return true;
        result = _defaultValue;
        return false;
    }
    /// <inheritdoc />
    protected override bool TryParse(ReadOnlySpan<byte> bytes, out TResult result)
    {
        // 只实现基类,并不实际调用
        result = _defaultValue;
        return false;
    }
}
