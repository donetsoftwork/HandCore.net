using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class EntityParserTests
{
    [Fact]
    public void WithProperty()
    {
        var id = 123;
        var name = "Jxj";
        var state = true;
        string json = @$"{{ ""Id"": {id}, ""Name"": ""{name}"",  ""State"": true}}";

        var config = HandJson.Default;
        var userParser = config.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty(config.Bool(), nameof(User.State));
        var result = userParser.Parse(json);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(state, result.State);
    }
}
