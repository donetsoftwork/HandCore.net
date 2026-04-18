using Hand.Maping;

namespace ProjectionsTests;

public class TrimProjectionTests
{
    [Theory]
    [InlineData(' ', " Id ", "Id")]
    [InlineData('_', "_name", "name")]
    [InlineData('s', "Users", "User")]
    public void Convert(char trimChar, string source, string expected)
    {
        char[] trimChars = [trimChar]; 
        var projection = new TrimProjection(trimChars);
        Assert.Equal(trimChars, projection.TrimChars);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
        
    }
}
