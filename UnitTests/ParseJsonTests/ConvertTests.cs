using Hand.ParseJson;
using ParseJsonTests.Supports;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class ConvertTests
{
    [Fact]
    public void Single()
    {
        var expected = 123;
        string jsonContent = "{\"id\": 123}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonContent));
        var config = HandJson.Default;
        var idReader = config.First<int>("id")
            .Convert(id => new UserId(id));
        var result = idReader.Get(reader);
        Assert.Equal(expected, result.Original);
    }

}
