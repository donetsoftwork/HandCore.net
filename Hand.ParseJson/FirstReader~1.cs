using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 第一个属性读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class FirstReader<TResult>(IJsonParser<TResult> original, TResult defaultValue)
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
        var currentDepth = reader.CurrentDepth;
        do
        {
            if (_original.TryParse(ref reader, out result))
                return true;
        } while (reader.Read() && reader.CurrentDepth >= currentDepth);
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
