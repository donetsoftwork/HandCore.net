using Hand.ParseXml;
using System.Xml;

namespace ParseXmlTests;

public class FirstReaderTests
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

        var summaryReader = HandXml.Default.Element("summary")
            .First();
        string summary = summaryReader.Parse(text);
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

        var config = HandXml.Default;
        var summaryReader = config.Element("summary", config.Content())
            .First();
        var result = summaryReader.Parse(text);
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

        var idParser = HandXml.Default.Element<int>("Id")
            .First();
        var result = idParser.Parse(text);
        Assert.Equal(id, result);
    }
    [Fact]
    public void AttributeName()
    {
        var expected = "F:GenerateConvertTests.Supports.ColumnType.Identity";
        var text = @$"<member name = ""{expected}"">
            <summary>
            自增列
            </summary>
        </member>";

        var nameReader = HandXml.Default.Attribute("name")
            .First();
        string name = nameReader.Parse(text);
        Assert.Equal(expected, name);
    }
    [Fact]
    public void AttributeName2()
    {
        var expected = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""{expected}"">{name}</User>";

        var idReader = HandXml.Default.Attribute<int>("name")
            .First();
        int result = idReader.Parse(text);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void AttributeIndex()
    {
        var expected = "F:GenerateConvertTests.Supports.ColumnType.Identity";
        var text = @$"<member name = ""{expected}"">
            <summary>
            自增列
            </summary>
        </member>";
        var nameReader = HandXml.Default.Attribute(0)
            .Element("member")
            .First();
        string name = nameReader.Parse(text);
        Assert.Equal(expected, name);
    }
    [Fact]
    public void AttributeIndex2()
    {
        var expected = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""{expected}"">{name}</User>";
        var idReader = HandXml.Default.Attribute<int>(0)
            .Element("User")
            .First();
        int result = idReader.Parse(text);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Name()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    </User>";
        var eachParser = HandXml.Default.Name()
            .First();
        var result = eachParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal("User", result);
    }
    [Fact]
    public void Name2()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    </User>";
        var eachParser = HandXml.Default.Name()
            .First()
            .MoveIn();
        var result = eachParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal("Id", result);
    }
    [Fact]
    public void Name3()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <true>
    	<Name>张三</Name>
    </true>";
        var eachParser = HandXml.Default.Name<bool>()
            .First();
        var result = eachParser.Parse(text);
        Assert.True(result);
    }
}
