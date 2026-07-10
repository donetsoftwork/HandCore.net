using Hand.ParseJson;
using ParseJsonTests.Supports;
using System.Text;

namespace ParseJsonTests;

public class ConvertTests
{
    [Fact]
    public void Single()
    {
        var expected = 123;
        string json = "{\"id\": 123}";

        var config = HandJson.Default;
        var idReader = config.First<int>("id")
            .Convert(id => new UserId(id));
        var result = idReader.Parse(json);
        Assert.Equal(expected, result.Original);
    }

}
