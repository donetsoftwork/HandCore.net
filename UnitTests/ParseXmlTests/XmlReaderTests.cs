using System.Xml;

namespace ParseXmlTests;

public class XmlReaderTests
{
    [Fact]
    public void Text()
    {
        string xmlContent = "<root><child>Text</child><!-- This is a comment --><child><![CDATA[<B>CDATA</B>]]></child></root>";
        using var reader = XmlReader.Create(new StringReader(xmlContent));
        string elementName = reader.Name;
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    elementName = reader.Name;
                    break;
                case XmlNodeType.Text:
                    var text = reader.Value;
                    Console.WriteLine("Text:"+text);
                    break;
                case XmlNodeType.CDATA:
                    var data = reader.Value;
                    Console.WriteLine("CDATA:" + data);
                    break;
                case XmlNodeType.Comment:
                    var comment = reader.Value;
                    Console.WriteLine("Comment:" + comment);
                    break;
                case XmlNodeType.EndElement:
                    var name2 = reader.Name;
                    break;
                default:
                    break;
            }
        }
    }
}
