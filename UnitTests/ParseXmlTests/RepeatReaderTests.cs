using Hand.ParseXml;
using ParseXmlTests.Supports;

namespace ParseXmlTests;

public class RepeatReaderTests
{
    [Fact]
    public void ListEntity()
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
        var elementReader = HandXml.Default.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            .WithItem<int>(nameof(User.Age))
            .Repeat(nameof(User));
        User[] result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void ListContent()
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
        var elementReader = HandXml.Default.Content()
            .Repeat(nameof(User.Name));
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void ListFirst()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>
    <User>
    	<Id>2</Id>
    	<Name>李四</Name>
    	<Age>11</Age>
    </User>
    <User>
    	<Id>3</Id>
    	<Name><![CDATA[<B>王二</B>]]></Name>
    	<Age>9</Age>
    </User>
    </Users>";
        var elementReader = HandXml.Default.First<int>(nameof(User.Id))
            .Repeat(nameof(User));
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void ListWithAttribute()
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
        var elementReader = HandXml.Default.Entity<User>()
            // 缺失属性填充默认值
            .WithAttribute<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            // 缺失节点和空节点填充默认值
            .WithItem<int>(nameof(User.Age))
            .Repeat();
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length); ;
    }
    [Fact]
    public void ListAttribute()
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
        var elementReader = HandXml.Default.Attribute<int>(nameof(User.Id))
            .Repeat(nameof(User));
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
}
