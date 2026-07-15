using Hand.Convert;
using Hand.ParseXml.Nodes;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 多节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="item"></param>
public class EachReader<TResult>(IParser<XmlReader, TResult> item)
     : WrapParser<TResult>(item), IParser<XmlReader, IEnumerable<TResult>>
    , IDataGet<XmlReader, IEnumerable<TResult>>, IDataGet<string, IEnumerable<TResult>>
{
    #region 配置
    private readonly IParser<XmlReader, TResult> _item = item;

    /// <summary>
    /// 子元素解析器
    /// </summary>
    public IParser<XmlReader, TResult> Item
        => _item;
    #endregion

    /// <inheritdoc />
    public IEnumerable<TResult> Get(XmlReader reader)
    {
        var depth = reader.Depth;
        do
        {
            if (_item.TryParse(reader, out var itemResult))
                yield return itemResult;
        }
        while (reader.Read() && reader.Depth >= depth);
    }
    /// <inheritdoc />
    public IEnumerable<TResult> Get(string text)
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. Get(xmlReader)];
    }

    /// <inheritdoc />
    public bool TryParse(XmlReader reader, out IEnumerable<TResult> result)
    {
        result = Get(reader);
        return true;
    }
}
