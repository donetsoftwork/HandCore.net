using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 解析标签名
/// </summary>
/// <param name="defaultValue"></param>
public class ElementNameParser(string defaultValue)
    : ValueReader<string>(defaultValue)
{
    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out string result)
    {
        if (reader.NodeType == XmlNodeType.Element)
        {
            result = reader.LocalName;
            return true;
        }
        result = _defaultValue; 
        return false;
    }
}
