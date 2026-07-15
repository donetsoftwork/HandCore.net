using Hand.ParseXml;
using Hand.ParseXml.Nodes;
using ParseXmlTests.Supports;
using System.Xml;

namespace ParseXmlTests;

public class DictionaryParserTests
{
    [Fact]
    public void Element()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Users>
            <User>
    	        <Id>1</Id>
    	        <Name>张三</Name>
            </User>
            <User>
    	        <Id>2</Id>
            </User>
            <User>
    	        <Id>3</Id>
    	        <Name><![CDATA[<B>王二</B>]]></Name>
            </User>
            </Users>";
        var config = HandXml.Default;
        var dictionaryReader = config.Element<int>(nameof(User.Id))
            .Dictionary(config.Element("Name").First());
        IDictionary<int,string> result = dictionaryReader.Get(text);
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public void AcceptDefault()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Users>
            <User>
    	        <Id>1</Id>
    	        <Name>张三</Name>
            </User>
            <User>
    	        <Id>2</Id>
            </User>
            <User>
    	        <Id>3</Id>
    	        <Name><![CDATA[<B>王二</B>]]></Name>
            </User>
            </Users>";
        var config = HandXml.Default;
        var dictionaryReader = config.Element<int>(nameof(User.Id))
            .Dictionary(config.Element("Name").First(), true);
        IDictionary<int, string> result = dictionaryReader.Get(text);
        Assert.Equal(3, result.Count);
    }
    [Fact]
    public void Dictionary()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <User>
    	        <Id>1</Id>
    	        <Name>张三</Name>
    	        <Age>10</Age>
            </User>";
        var config = HandXml.Default;
        var dictionaryReader = config.Name()
            .Dictionary(config.Content().First())
            // 跳过当前User节点
            .MoveIn()
            .Element("User")
            .First();
        IDictionary<string,string> result = dictionaryReader.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }
    [Fact]
    public void Dictionary2()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>";
        var config = HandXml.Default;
        var dictionaryReader = config.Name()
            .Dictionary(config.Content().First());
        // 错误示范,User节点不能读取,会抛异常
        // 参考Dictionary,跳过节点
        Assert.ThrowsAny<XmlException>(() => dictionaryReader.Parse(text));
    }
}
