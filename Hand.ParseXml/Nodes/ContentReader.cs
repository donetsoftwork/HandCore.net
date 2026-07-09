using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 文本读取器
/// </summary>
public class ContentReader(string defaultValue)
    : ValueReader<string>(defaultValue)
{
    ///// <inheritdoc />
    //public override string Get(XmlReader reader)
    //{
    //    if (reader.IsEmptyElement)
    //        return _defaultValue;
    //    return reader.ReadElementContentAsString();
    //}
    /// <inheritdoc />
    public override bool TryParser(XmlReader reader, out string result)
    {
        if (reader.IsEmptyElement)
        {
            result = _defaultValue;
            return false;
        }
        result = reader.ReadElementContentAsString();
        return true;
    }
}
