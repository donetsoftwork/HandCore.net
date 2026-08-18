using Hand.Maping;

namespace ProjectionsTests;

public class DictionaryProjectionTests
{
    private readonly IProjection<string>  _projection = new DictionaryProjection<string>(new Dictionary<string, string>
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
        var reversed = _projection.Reverse();
        reversed.TryConvert(expected, out var source2);
        Assert.Equal(source, source2);
    }
}
