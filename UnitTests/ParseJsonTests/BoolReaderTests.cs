using Hand.ParseJson;
using Hand.ParseJson.Primitives;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class BoolReaderTests
{
    [Fact]
    public void Bool()
    {
        var expected = true;
        string jsonContent = "{\"state\": true}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", config.Bool());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool2()
    {
        var expected = true;
        string jsonContent = "{\"state\": \"true\"}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", config.Bool());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool3()
    {
        var expected = true;
        string jsonContent = "{\"state\": true}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", config.Value<bool>());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool4()
    {
        var expected = true;
        string jsonContent = "{\"state\": \"true\"}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", config.Value<bool>());
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool5()
    {
        var expected = "赢";
        string jsonContent = "{\"state\": true}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", new BoolReader<string>("赢", "输", "平"));
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool6()
    {
        var expected = 3;
        string jsonContent = "{\"state\": true}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool7()
    {
        var expected = 1;
        string jsonContent = "{\"state\": null}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool8()
    {
        var expected = 1;
        string jsonContent = "{}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = idReader.Get(reader);
        Assert.Equal(expected, result);
    }
}
