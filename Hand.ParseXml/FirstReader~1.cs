using Hand.Convert;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Nodes;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 第一个节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="element"></param>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class FirstReader<TResult>(string element, IParser<XmlReader, TResult> original, TResult defaultValue)
    : ValueReader<TResult>(defaultValue)
{
    #region 配置
    private readonly string _element = element;
    private readonly IParser<XmlReader, TResult> _original = original;

    /// <summary>
    /// 标签名
    /// </summary>
    public string Element
        => _element;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IParser<XmlReader, TResult> Original
        => _original;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out TResult result)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                // 匹配第一个
                if (reader.Name == _element)
                {
                    if(_original.TryParse(reader, out result))
                        return true;
                    break;
                }
            }
        }
        result = _defaultValue;
        return false;
    }
}
