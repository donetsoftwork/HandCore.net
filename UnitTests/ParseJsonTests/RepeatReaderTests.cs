using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class RepeatReaderTests
{
    [Fact]
    public void ListEntity()
    {
        string json = "[{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}, { \"Id\": 2, \"Name\": \"李四\",  \"State\": false}]";

        var repeatReader = HandJson.Default.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State))
            .Repeat();
        var result = repeatReader.Parse(json);
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public void ListSingle()
    {
        string json = @$"[{{ ""Id"": 1}}, {{ ""Id"": 2}}]";

        var repeatReader = HandJson.Default.First<int>(nameof(User.Id))
            .Repeat()
            .ToArray();
        var result = repeatReader.Parse(json);
        Assert.Equal(2, result.Length);
    }    
}
