using Hand.Maping;

namespace ProjectionsTests;

public class TrimEndProjectionTests
{
    [Theory]
    [InlineData(' ', " Id ", " Id")]
    [InlineData('_', "_name", "_name")]
    [InlineData('s', "Users", "User")]
    public void Convert(char trimChar, string source, string expected)
    {
        char[] trimChars = [trimChar];
        var projection = new TrimEndProjection(trimChars);
        Assert.Equal(trimChars, projection.Ends);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
