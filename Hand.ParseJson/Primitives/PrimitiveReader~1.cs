using Hand.Maping;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson.Primitives;

/// <summary>
/// 基础类型读取器
/// </summary>
/// <param name="converter"></param>
/// <param name="defaultValue"></param>
public class PrimitiveReader<TValue>(ISpanConverter<byte, TValue> converter, TValue defaultValue)
    : ValueReader<TValue>(defaultValue)
{
    #region 配置
    /// <summary>
    /// ReadOnlySpan转化器
    /// </summary>
    private readonly ISpanConverter<byte, TValue> _converter = converter;
    /// <summary>
    /// ReadOnlySpan转化器
    /// </summary>
    public ISpanConverter<byte, TValue> Converter
        => _converter;
    #endregion

    /// <inheritdoc />
    protected override TValue GetValue(ref Utf8JsonReader reader)
        => _converter.Convert(GetOriginalValue(ref reader));
}