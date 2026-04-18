using Hand.Maping;

namespace ProjectionsTests;

public class TrimStartProjectionTests
{
    [Theory]
    [InlineData(' ', " Id ", "Id ")]
    [InlineData('_', "_name", "name")]
    [InlineData('s', "Users", "Users")]
    public void Convert(char trimChar, string source, string expected)
    {
        char[] trimChars = [trimChar];
        var projection = new TrimStartProjection(trimChars);
        Assert.Equal(trimChars, projection.Starts);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
