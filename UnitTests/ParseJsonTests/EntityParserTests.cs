using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class EntityParserTests
{
    [Fact]
    public void Entity()
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
    [Fact]
    public void First()
    {
        var id = 123;
        var name = "Jxj";
        var state = true;
        string json = @$"{{ ""Id"": {id}, ""Name"": ""{name}"",  ""State"": true}}";

        var config = HandJson.Default;
        var userParser = config.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty(config.Bool(), nameof(User.State))
            .Object()
            .First();
        var result = userParser.Parse(json);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(state, result.State);
    }
    [Fact]
    public void First2()
    {
        string json = "[{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}, { \"Id\": 2, \"Name\": \"李四\",  \"State\": false}]";

        var repeatReader = HandJson.Default.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State))
            .Object()
            .First();
        var result = repeatReader.Parse(json);
        Assert.Equal(1, result.Id);
        Assert.Equal("张三", result.Name);
        Assert.True(result.State);
    }
    [Fact]
    public void First3()
    {
        string json = "[{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}, { \"Id\": 2, \"Name\": \"李四\",  \"State\": false}]";

        var repeatReader = HandJson.Default.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State))
            .First();
        var result = repeatReader.Parse(json);
        // 这是个错误的示范
        // 从结果看读取到了第二个
        // 实际是两个都读取了,第一个完全被第二个覆盖了
        // 这个情况需要按First2示例,加上Object()和First()
        Assert.Equal(2, result.Id);
        Assert.Equal("李四", result.Name);
        Assert.False(result.State);
    }
}
