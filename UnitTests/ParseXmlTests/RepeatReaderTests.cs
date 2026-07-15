using Hand.ParseXml;
using ParseXmlTests.Supports;

namespace ParseXmlTests;

public class EachReaderTests
{
    [Fact]
    public void EachEntity()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Users>
            <User>
    	        <Id>1</Id>
    	        <Name>张三</Name>
            </User>
            <User>
    	        <Id>2</Id>
    	        <Name>李四</Name>
            </User>
            <User>
    	        <Id>3</Id>
    	        <Name><![CDATA[<B>王二</B>]]></Name>
            </User>
            </Users>";
        var eachParser = HandXml.Default.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            .WithItem<int>(nameof(User.Age))
            .Element(nameof(User))
            .Each();
        User[] result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachContent()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
    	<Name>张三</Name>
    </User>
    <User>
    	<Name>李四</Name>
    </User>
    <User>
    	<Name><![CDATA[<B>王二</B>]]></Name>
    </User>
    </Users>";
        var eachParser = HandXml.Default.Content()
            .Element(nameof(User.Name))
            .Each();
        var result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachElement()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
    	<Id>1</Id>
    </User>
    <User>
    	<Id>2</Id>
    </User>
    <User>
    	<Id>3</Id>
    </User>
    </Users>";
        var eachParser = HandXml.Default.Element<int>(nameof(User.Id))
            .Each();
        var result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachFirst()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
    	<Id>1</Id>
    </User>
    <User>
    	<Id>2</Id>
    </User>
    <User>
    	<Id>3</Id>
    </User>
    </Users>";
        var eachParser = HandXml.Default.Element<int>(nameof(User.Id))
            .First()
            .Element("User")
            .Each();
        var result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachWithAttribute()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User Id=""1"">
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>
    <User>
    	<Name>李四</Name>
    </User>
    <User Id=""3"">
    	<Name>王二</Name>
    	<Age />
    </User>
    </Users>";
        var eachParser = HandXml.Default.Entity<User>()
            // 缺失属性填充默认值
            .WithAttribute<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            // 缺失节点和空节点填充默认值
            .WithItem<int>(nameof(User.Age))
            .Element(nameof(User))
            .Each();
        var result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length); ;
    }
    [Fact]
    public void EachAttribute()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User Id=""1"">
    	<Name>张三</Name>
    </User>
    <User Id=""2"">
    	<Name>李四</Name>
    </User>
    <User Id=""3"">
    	<Name>王二</Name>
    </User>
    </Users>";
        var eachParser = HandXml.Default.Attribute<int>(nameof(User.Id))
            .Element(nameof(User))
            .Each();
        var result = eachParser.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachName()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>";
        var config = HandXml.Default;
        var eachParser = config.Name()
            .Each()
            .MoveIn();
        var result = eachParser.Parse(text)
            .ToArray();
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void EachName2()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>";
        var config = HandXml.Default;
        var eachParser = config.Name()
            .Each()
            .MoveTo("Id");
        var result = eachParser.Parse(text)
            .ToArray();
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
    }
}
