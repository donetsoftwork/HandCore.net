using Hand.ParseJson;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class SingleReaderTests
{
    [Fact]
    public void Single()
    {
        var expected = 123;
        string jsonContent = "{\"id\": 123}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First<int>("id");
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Single2()
    {
        var expected = "123";
        string jsonContent = "{\"id\": 123}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("id");
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void SingleValue()
    {
        var expected = 123;
        string jsonContent = "{\"id\": 123}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("id", config.Value<int>());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String()
    {
        var expected = "jxj";
        string jsonContent = "{\"name\": \"jxj\"}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("name", config.String());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String2()
    {
        var expected = "123";
        string jsonContent = "{\"id\": 123}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("id", config.String());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void String3()
    {
        var expected = "true";
        string jsonContent = "{\"state\": true}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", config.String());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    // 拆分到 BoolReaderTests
    //[Fact]
    //public void Bool()
    //{
    //    var expected = true;
    //    string jsonContent = "{\"state\": true}";
    //    var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
    //    var config = HandJson.Default;
    //    var idReader = config.First("state", config.Bool());
    //    var result = idReader.Get(reader);
    //    Assert.Equal(expected, result);
    //}
    //[Fact]
    //public void Bool2()
    //{
    //    var expected = true;
    //    string jsonContent = "{\"state\": true}";
    //    var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
    //    var config = HandJson.Default;
    //    var idReader = config.First<bool>("state");
    //    var result = idReader.Get(reader);
    //    Assert.Equal(expected, result);
    //}
}
