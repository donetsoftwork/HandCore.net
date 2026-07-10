using Hand.ParseJson;

namespace ParseJsonTests;

public class SingleReaderTests
{
    [Fact]
    public void Single()
    {
        var expected = 123;
        string json = "{\"id\": 123}";

        var idReader = HandJson.Default.First<int>("id");
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Single2()
    {
        var expected = "123";
        string json = "{\"id\": 123}";

        var idReader = HandJson.Default.First("id");
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void SingleValue()
    {
        var expected = 123;
        string json = "{\"id\": 123}";

        var config = HandJson.Default;
        var idReader = config.First("id", config.Value<int>());
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String()
    {
        var expected = "jxj";
        string json = "{\"name\": \"jxj\"}";

        var config = HandJson.Default;
        var idReader = config.First("name", config.String());
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String2()
    {
        var expected = "123";
        string json = "{\"id\": 123}";

        var config = HandJson.Default;
        var idReader = config.First("id", config.String());
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String3()
    {
        var expected = "true";
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var idReader = config.First("state", config.String());
        var result = idReader.Parse(json);
        Assert.Equal(expected, result);
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
