using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class FirstReaderTests
{
    [Fact]
    public void First()
    {
        var expected = 123;
        string json = "{\"id\": 123}";

        var idReader = HandJson.Default.First<int>("id");
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void First2()
    {
        var expected = "123";
        string json = "{\"id\": 123}";

        var idReader = HandJson.Default.First("id");
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Property()
    {
        var expected = "jxj";
        string json = "{\"name\": \"jxj\"}";

        var config = HandJson.Default;
        var nameReader = config.Property("name", config.String())
            .First();
        var result = nameReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Property2()
    {
        var expected = "123";
        string json = "{\"id\": 123}";

        var config = HandJson.Default;
        var idReader = config.Property("id", config.String())
            .First();
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Property3()
    {
        var expected = "true";
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var stateReader = config.Property("state", config.String())
            .First();
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Property4()
    {
        string json = @$"[{{ ""Id"": 1}}, {{ ""Id"": 2}}]";

        var firsttReader = HandJson.Default.Property<int>(nameof(User.Id))
            .First();
        var result = firsttReader.Parse(json);
        Assert.Equal(1, result);
    }
    [Fact]
    public void PropertyName()
    {
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var nameReader = config.PropertyName()
            .First();
        var result = nameReader.Parse(json);
        Assert.Equal("state", result);
    }
    // 拆分到 BoolReaderTests
    //[Fact]
    //public void Bool()
    //{
    //    var expected = true;
    //    string json = "{\"state\": true}";
    //    var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
    //    var config = HandJson.Default;
    //    var idReader = config.First("state", config.Bool());
    //    var result = idReader.Get(reader);
    //    Assert.Equal(expected, result);
    //}
}
