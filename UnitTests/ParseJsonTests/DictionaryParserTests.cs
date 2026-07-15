using Hand.ParseJson;
using ParseJsonTests.Supports;

namespace ParseJsonTests;

public class DictionaryParserTests
{
    [Fact]
    public void Property()
    {
        string json = "[{ \"Id\": 1, \"Name\": \"张三\"}, { \"Id\": 2, \"Name\": \"李四\"}]";

        var config = HandJson.Default;
        var dictionaryReader = config.Property<int>(nameof(User.Id))            
            .Dictionary(config.Property("Name").First());
        IDictionary<int, string> result = dictionaryReader.Parse(json);
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public void PropertyName()
    {
        string json = "{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}";
        var config = HandJson.Default;
        var dictionaryReader = config.PropertyName()
            .Dictionary(config.String());
        IDictionary<string, string> result = dictionaryReader.Parse(json);
        Assert.Equal(3, result.Count);
    }
    [Fact]
    public void Dictionary()
    {
        string json = "{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}";

        var dictionaryReader = HandJson.Default.Dictionary();
        IDictionary<string, object> result = dictionaryReader.Parse(json);
        Assert.Equal(3, result.Count);
    }
    [Fact]
    public void Dictionary2()
    {
        string json = "{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}";

        var dictionaryReader = HandJson.Default.Dictionary<string>();
        IDictionary<string, string> result = dictionaryReader.Parse(json);
        Assert.Equal(3, result.Count);
    }
    [Fact]
    public void Dictionary3()
    {
        string json = "{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}";

        var dictionaryReader = HandJson.Default.Dictionary<string, string>();
        IDictionary<string, string> result = dictionaryReader.Parse(json);
        Assert.Equal(3, result.Count);
    }
}
