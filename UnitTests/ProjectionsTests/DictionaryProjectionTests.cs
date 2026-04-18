using Hand.Maping;

namespace ProjectionsTests;

public class DictionaryProjectionTests
{
    private DictionaryProjection<string> _projection = new(new Dictionary<string, string>
    {
        { "feet", "foot" },
        { "mice", "mouse" }
     });

    [Theory]
    [InlineData("feet", "foot")]
    [InlineData("mice", "mouse")]
    public void Convert(string source, string expected)
    {
        _projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
