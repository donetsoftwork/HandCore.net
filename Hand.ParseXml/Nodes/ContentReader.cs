using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 文本读取器
/// </summary>
public class ContentReader(string defaultValue)
    : ValueReader<string>(defaultValue)
{
    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out string result)
    {
        reader.MoveToContent();
        if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
        {
            //reader.CanResolveEntity =
            result = reader.ReadElementContentAsString();
            // result = reader.ReadContentAsString();
            return true;
        }
        result = _defaultValue;
        return false;
    }
}
