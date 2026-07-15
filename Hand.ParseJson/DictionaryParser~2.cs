using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 字典解析器
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="key"></param>
/// <param name="value"></param>
/// <param name="comparer"></param>
/// <param name="acceptDefault"></param>
public class DictionaryParser<TKey, TValue>(IJsonParser<TKey> key, IJsonParser<TValue> value, IEqualityComparer<TKey> comparer, bool acceptDefault)
    : IJsonParser<IDictionary<TKey, TValue>>
    where TKey : notnull
{
    private readonly IJsonParser<TKey> _key = key;
    private readonly IJsonParser<TValue> _value = value;
    private readonly IEqualityComparer<TKey> _comparer = comparer;
    private readonly JsonReadItemAction<TKey, TValue> _readAction = acceptDefault ? ReadItemDefault : TryReadItem;

    /// <summary>
    /// 构造新对象
    /// </summary>
    /// <returns></returns>
    public virtual IDictionary<TKey, TValue> New()
        => new Dictionary<TKey, TValue>(_comparer);
    /// <inheritdoc />
    public bool TryParse(ref Utf8JsonReader reader, out IDictionary<TKey, TValue> result)
    {
        var state = false;        
        result = New();
        var currentDepth = reader.CurrentDepth;
        do
        {
            if (ReadItem(ref reader, result))
                state = true;
        }
        while (reader.Read() && reader.CurrentDepth >= currentDepth);
        return state;
    }
    /// <summary>
    /// 读取子项
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    protected virtual bool ReadItem(ref Utf8JsonReader reader, IDictionary<TKey, TValue> dictionary)
        => _readAction(_key, _value, ref reader, dictionary);
    /// <summary>
    /// 尝试读取子项
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    protected static bool TryReadItem(IJsonParser<TKey> key, IJsonParser<TValue> value, ref Utf8JsonReader reader, IDictionary<TKey, TValue> dictionary)
    {
        if (key.TryParse(ref reader, out var keyResult)
            && value.TryParse(ref reader, out var valueResult))
        {
            dictionary[keyResult] = valueResult;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 读取子项默认值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    protected static bool ReadItemDefault(IJsonParser<TKey> key, IJsonParser<TValue> value, ref Utf8JsonReader reader, IDictionary<TKey, TValue> dictionary)
    {
        if (key.TryParse(ref reader, out var keyResult))
        {
            dictionary[keyResult] = value.Parse(ref reader);
            return true;
        }
        return false;
    }
}
