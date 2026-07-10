using Hand.ParseXml;
using ParseXmlTests.Supports;
using System.Xml;

namespace ParseXmlTests;

public class CustomerTests
{
    [Fact]
    public void WithNode()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<Customer>
	<CustomerId>{id}</CustomerId>
	<CustomerName>{name}</CustomerName>
	<CustomerAge>{age}</CustomerAge>
</Customer>";

        var userParser = HandXml.Default.Entity<Customer>()
            .WithItem<int>(nameof(Customer.CustomerId))
            .WithItem(nameof(Customer.CustomerName))
            .WithItem<int>(nameof(Customer.CustomerAge));
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.CustomerId);
        Assert.Equal(name, result.CustomerName);
        Assert.Equal(age, result.CustomerAge);
    }
    [Fact]
    public void WithNode2()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<User>
	<Id>{id}</Id>
	<Name>{name}</Name>
	<Age>{age}</Age>
</User>";

        var userParser = HandXml.Default.Entity<Customer>()
            .WithItem<int>("Id", nameof(Customer.CustomerId))
            .WithItem("Name", nameof(Customer.CustomerName))
            .WithItem<int>("Age", nameof(Customer.CustomerAge))
            .First("User");
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.CustomerId);
        Assert.Equal(name, result.CustomerName);
        Assert.Equal(age, result.CustomerAge);
    }
    [Fact]
    public void WithAttribute()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""{id}"" Name=""{name}"" Age=""{age}"" />";

        var userParser = HandXml.Default.Entity<Customer>()
            .WithAttribute<int>("Id", nameof(Customer.CustomerId))
            .WithAttribute("Name", nameof(Customer.CustomerName))
            .WithAttribute<int>("Age", nameof(Customer.CustomerAge))
            .First("User");
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.CustomerId);
        Assert.Equal(name, result.CustomerName);
        Assert.Equal(age, result.CustomerAge);
    }
    [Fact]
    public void ListNode()
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

        var elementReader = HandXml.Default.Entity<Customer>()
            .WithItem<int>("Id", nameof(Customer.CustomerId))
            .WithItem("Name", nameof(Customer.CustomerName))
            .WithItem<int>("Age", nameof(Customer.CustomerAge))
            .Repeat("User");
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length);
    }
    [Fact]
    public void ListAttribute()
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

        var elementReader = HandXml.Default.Entity<Customer>()
            // 缺失属性填充默认值
            .WithAttribute<int>("Id", nameof(Customer.CustomerId))
            .WithItem("Name", nameof(Customer.CustomerName))
            // 缺失节点和空节点填充默认值
            .WithItem<int>("Age", nameof(Customer.CustomerAge))
            .Repeat("User");
        var result = elementReader.Get(text)
            .ToArray();
        Assert.Equal(3, result.Length); ;
    }
}
