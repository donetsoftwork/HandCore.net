using Hand.ParseJson;
using ParseJsonTests.Supports;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class EntityParserTests
{
    [Fact]
    public void WithProperty()
    {
        var id = 123;
        var name = "Jxj";
        var state = true;
        string jsonContent = @$"{{ ""Id"": {id}, ""Name"": ""{name}"",  ""State"": true}}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var userParser = config.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State));
        var result = userParser.Get(reader);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(state, result.State);
    }
}
