using Hand.Convert;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 字典解析器
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="key"></param>
/// <param name="value"></param>
/// <param name="comparer"></param>
/// <param name="acceptDefault"></param>
public class DictionaryParser<TKey, TValue>(IParser<XmlReader, TKey> key, IParser<XmlReader, TValue> value, IEqualityComparer<TKey> comparer, bool acceptDefault)
    : IParser<XmlReader, IDictionary<TKey, TValue>>, IDataGet<XmlReader, IDictionary<TKey, TValue>>
    where TKey : notnull
{
    private readonly IParser<XmlReader, TKey> _key = key;
    private readonly IParser<XmlReader, TValue> _value = value;
    private readonly IEqualityComparer<TKey> _comparer = comparer;
    private readonly Action<IParser<XmlReader, TKey>, IParser<XmlReader, TValue>, XmlReader, IDictionary<TKey, TValue>> _readAction = acceptDefault ? ReadItemDefault : TryReadItem;

    /// <summary>
    /// 构造新对象
    /// </summary>
    /// <returns></returns>
    public virtual IDictionary<TKey, TValue> New()
        => new Dictionary<TKey, TValue>(_comparer);

    /// <inheritdoc />
    public IDictionary<TKey, TValue> Get(XmlReader reader)
    {
        var dictionary = New();
        var depth = reader.Depth;
        do
        {
            ReadItem(reader, dictionary);
        }
        while (reader.Read() && reader.Depth >= depth);
        return dictionary;
    }
    /// <summary>
    /// 读取子项
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    protected virtual void ReadItem(XmlReader reader, IDictionary<TKey, TValue> dictionary)
        => _readAction(_key, _value, reader, dictionary);
    /// <summary>
    /// 尝试读取子项
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    protected static void TryReadItem(IParser<XmlReader, TKey> key, IParser<XmlReader, TValue> value, XmlReader reader, IDictionary<TKey, TValue> dictionary)
    {
        if (key.TryParse(reader, out var keyResult) && value.TryParse(reader, out var valueResult))
            dictionary[keyResult] = valueResult;
    }
    /// <summary>
    /// 读取子项默认值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reader"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    protected static void ReadItemDefault(IParser<XmlReader, TKey> key, IParser<XmlReader, TValue> value, XmlReader reader, IDictionary<TKey, TValue> dictionary)
    {
        if (key.TryParse(reader, out var keyResult))
            dictionary[keyResult] = value.Parse(reader);
    }
    /// <inheritdoc />
    public bool TryParse(XmlReader reader, out IDictionary<TKey, TValue> result)
    {
        result = Get(reader);
        return true;
    }
}
