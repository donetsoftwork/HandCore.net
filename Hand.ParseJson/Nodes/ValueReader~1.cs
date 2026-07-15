using Hand.Configuration;
using Hand.ParseJson.Contracts;
using System.Buffers;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 值读取器基类
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class ValueReader<TValue>(TValue defaultValue)
    : IJsonParser<TValue>, IDefault<TValue>
{
    #region 配置
    /// <summary>
    /// 默认值
    /// </summary>
    protected readonly TValue _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public TValue DefaultValue
        => _defaultValue;
    #endregion
    /// <summary>
    /// 获取原始值
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected ReadOnlySpan<byte> GetOriginalValue(ref Utf8JsonReader reader)
        => reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    protected abstract bool TryParse(ReadOnlySpan<byte> bytes, out TValue result);
    /// <inheritdoc />
    public virtual bool TryParse(ref Utf8JsonReader reader, out TValue result)
    {
        if (reader.Read() && TryParse(GetOriginalValue(ref reader), out result))
            return true;
        result = _defaultValue;
        return false;
    }
}
