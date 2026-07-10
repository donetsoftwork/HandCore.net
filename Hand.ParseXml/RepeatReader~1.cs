using Hand.Convert;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Nodes;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 多节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="xml"></param>
/// <param name="name"></param>
/// <param name="item"></param>
public class RepeatReader<TResult>(HandXml xml, string name, IParser<XmlReader, TResult> item)
     : WrapParser<TResult>(xml, item), IParser<XmlReader, IEnumerable<TResult>>
    , IDataGet<XmlReader, IEnumerable<TResult>>, IDataGet<string, IEnumerable<TResult>>
{
    #region 配置
    private readonly string _name = name;
    private readonly IParser<XmlReader, TResult> _item = item;

    /// <summary>
    /// 标签名
    /// </summary>
    public string Name
        => _name;
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
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == _name)
                        if (_item.TryParse(reader, out var itemResult))
                            yield return itemResult;
                    break;
                case XmlNodeType.EndElement:
                    // 检查是否到达结束节点
                    //if (_parent.Validate(reader))
                    if (reader.Depth <= depth)
                        yield break;
                    break;
                default:
                    break;
            }
        }
        while (reader.Read());
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
