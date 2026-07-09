using Hand.ParseXml;
using ParseXmlTests.Supports;
using System.Xml;

namespace ParseXmlTests;

public class SingleReaderTests
{
    [Fact]
    public void Summary()
    {
        var expected = "自增列";
        var text = @$"<member name = ""F:GenerateConvertTests.Supports.ColumnType.Identity"">
            <summary>
            {expected}
            </summary>
        </member>";
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        var config = HandXml.Default;
        var summaryReader = config.First("summary");
        string summary = summaryReader.Get(xmlReader);
        Assert.Equal(expected, summary.Trim());
    }
    [Fact]
    public void Summary0()
    {
        var expected = "自增列";
        var text = @$"<member name = ""F:GenerateConvertTests.Supports.ColumnType.Identity"">
            <summary>
            {expected}
            </summary>
        </member>";
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        var config = HandXml.Default;
        var summaryReader = config.First("summary", config.Content());
        var result = summaryReader.Get(xmlReader);
        Assert.Equal(expected, result.Trim());
    }
    [Fact]
    public void WithNode()
    {
        var id = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<User>
	<Id>{id}</Id>
	<Name>{name}</Name>
</User>";
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        var config = HandXml.Default;
        var idParser = config.First<int>("Id");
        var result = idParser.Get(xmlReader);
        Assert.Equal(id, result);
    }
    [Fact]
    public void Attribute()
    {
        var expected = "F:GenerateConvertTests.Supports.ColumnType.Identity";
        var text = @$"<member name = ""{expected}"">
            <summary>
            自增列
            </summary>
        </member>";
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        var nameReader = HandXml.Default.Attribute("name")
            .First("member");
        string name = nameReader.Get(xmlReader);
        Assert.Equal(expected, name);
    }
    [Fact]
    public void Attribute2()
    {
        var expected = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""{expected}"">{name}</User>";
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        var idReader = HandXml.Default.Attribute<int>("Id")
            .First("User");
        int result = idReader.Get(xmlReader);
        Assert.Equal(expected, result);
    }
}
