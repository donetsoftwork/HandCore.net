using Hand.ParseJson;
using Hand.ParseJson.Primitives;

namespace ParseJsonTests;

public class BoolReaderTests
{
    [Fact]
    public void Bool()
    {
        var expected = true;
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var stateReader = config.First("state", config.Bool());
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool2()
    {
        var expected = true;
        string json = "{\"state\": \"true\"}";

        var config = HandJson.Default;
        var stateReader = config.First("state", config.Bool());
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool3()
    {
        var expected = true;
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var stateReader = config.First("state", config.Value<bool>());
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool4()
    {
        var expected = true;
        string json = "{\"state\": \"true\"}";

        var config = HandJson.Default;
        var stateReader = config.First("state", config.Value<bool>());
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool5()
    {
        var expected = "赢";
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var stateReader = config.First("state", new BoolReader<string>("赢", "输", "平"));
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool6()
    {
        var expected = 3;
        string json = "{\"state\": true}";

        var config = HandJson.Default;
        var stateReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool7()
    {
        var expected = 1;
        string json = "{\"state\": null}";

        var config = HandJson.Default;
        var stateReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Bool8()
    {
        var expected = 1;
        string json = "{}";

        var config = HandJson.Default;
        var stateReader = config.First("state", new BoolReader<int>(3, 0, 1));
        var result = stateReader.Parse(json);
        Assert.Equal(expected, result);
    }
}
