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
    /// 获取值
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected abstract TValue GetValue(ref Utf8JsonReader reader);
    /// <inheritdoc />
    public virtual bool TryParser(ref Utf8JsonReader reader, out TValue result)
    {
        if (reader.Read())
        {
            result = GetValue(ref reader);
            return true;
        }            
        result = _defaultValue;
        return false;
    }
}
