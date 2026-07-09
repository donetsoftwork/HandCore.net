using Hand.ParseJson;
using ParseJsonTests.Supports;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class RepeatReaderTests
{
    [Fact]
    public void ListEntity()
    {
        string jsonContent = @$"[{{ ""Id"": 1, ""Name"": ""张三"",  ""State"": true}}, {{ ""Id"": 2, ""Name"": ""李四"",  ""State"": false}}]";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var elementReader = config.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State))
            .Repeat();
        var result = elementReader.Get(reader);
        Assert.Equal(2, result.Count);
    }
    //[Fact]
    //public void ListProperty()
    //{
    //    string jsonContent = @$"[{{ ""Id"": 1, ""Name"": ""张三"",  ""State"": true}}, {{ ""Id"": 2, ""Name"": ""李四"",  ""State"": false}}]";
    //    var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
    //    var config = HandJson.Default;
    //    var elementReader = config.Content()
    //        .Repeat(nameof(User.Name));
    //    var result = elementReader.Get(xmlReader)
    //        .ToArray();
    //    Assert.Equal(3, result.Length);
    //}
    [Fact]
    public void ListSingle()
    {
        string jsonContent = @$"[{{ ""Id"": 1}}, {{ ""Id"": 2}}]";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var elementReader = config.First<int>(nameof(User.Id))
            .Repeat();
        var result = elementReader.Get(reader);
        Assert.Equal(2, result.Count);
    }    
}
