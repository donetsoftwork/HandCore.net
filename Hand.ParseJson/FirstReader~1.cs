using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 第一个属性读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="name"></param>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class FirstReader<TResult>(string name, IJsonParser<TResult> original, TResult defaultValue)
    : ValueReader<TResult>(defaultValue)
{
    #region 配置
    private readonly string _name = name;
    private readonly IJsonParser<TResult> _original = original;

    /// <summary>
    /// 属性名
    /// </summary>
    public string Name
        => _name;
    /// <summary>
    /// 原始读取器
    /// </summary>
    public IJsonParser<TResult> Original 
        => _original;
    #endregion
    ///// <inheritdoc />
    //protected override TResult GetValue(ref Utf8JsonReader reader)
    //{
    //    // 该方法不会执行,只是实现基类
    //    return _defaultValue;
    //}
    /// <inheritdoc />
    public override bool TryParser(ref Utf8JsonReader reader, out TResult result)
    {
        do
        {
            if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(_name))
            {
                if(_original.TryParser(ref reader, out result))
                    return true;
                break;
            }
        } while (reader.Read());
        result = _defaultValue;
        return false;
    }
    /// <inheritdoc />
    protected override bool TryParser(ReadOnlySpan<byte> bytes, out TResult result)
    {
        // 只实现基类,并不实际调用
        result = _defaultValue;
        return false;
    }
}
