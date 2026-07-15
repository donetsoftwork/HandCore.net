using Hand.ParseXml;
using ParseXmlTests.Supports;

namespace ParseXmlTests;

public class EntityParserTests
{
    [Fact]
    public void WithNode()
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

        var userParser = HandXml.Default.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            .WithItem<int>(nameof(User.Age));
        User result = userParser.Get(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
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

        var userParser = HandXml.Default.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            .WithItem<int>(nameof(User.Age));
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
    }
    [Fact]
    public void WithAttribute()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{id}"" Name=""{name}"" Age=""{age}"" />";

        var userParser = HandXml.Default.Entity<User>()
            .WithAttribute<int>(nameof(User.Id))
            .WithAttribute(nameof(User.Name))
            .WithAttribute<int>(nameof(User.Age))
            .Element("User")
            .First();
        User result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
    }
    [Fact]
    public void Content()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{id}"" Age=""{age}"">{name}</User>";

        var userParser = HandXml.Default.Entity<User>(nameof(User.Name))
            .WithAttribute<int>(nameof(User.Id))
            .WithAttribute<int>(nameof(User.Age))
            .Element("User")
            .First();
        User result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
    }
    [Fact]
    public void Content0()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{id}"" Age=""{age}"">{name}</User>";

        var config = HandXml.Default;
        var userParser = config.Entity<User>(config.Content().Member(nameof(User.Name)))
            .WithAttribute<int>(nameof(User.Id))
            .WithAttribute<int>(nameof(User.Age))
            .Element("User")
            .First();
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
    }
}
