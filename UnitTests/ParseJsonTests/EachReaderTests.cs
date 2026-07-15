using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class EachReaderTests
{
    [Fact]
    public void ListEntity()
    {
        string json = "[{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}, { \"Id\": 2, \"Name\": \"李四\",  \"State\": false}]";

        var repeatReader = HandJson.Default.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State))
            .Object()
            .Each();
        var result = repeatReader.Parse(json);
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public void Property()
    {
        string json = @$"[{{ ""Id"": 1}}, {{ ""Id"": 2}}]";

        var repeatReader = HandJson.Default.Property<int>(nameof(User.Id))
            .Each()
            .ToArray();
        var result = repeatReader.Parse(json);
        Assert.Equal(2, result.Length);
    }
    [Fact]
    public void Property2()
    {
        string json = @$"[{{ ""Name"": ""张三""}}, {{ ""Name"": ""李四""}}]";

        var repeatReader = HandJson.Default.Property(nameof(User.Name))
            .Each()
            .ToArray();
        var result = repeatReader.Parse(json);
        Assert.Equal(2, result.Length);
    }
    [Fact]
    public void PropertyName()
    {
        string json = @$"{{ ""Id"": 1, ""Name"": ""张三"",  ""State"": true}}";

        var repeatReader = HandJson.Default.PropertyName()
            .Each()
            .ToArray();
        var result = repeatReader.Parse(json);
        Assert.Equal(3, result.Length);
    }
}
