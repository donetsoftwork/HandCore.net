using Hand.Maping;

namespace ProjectionsTests;

public class ReplaceSuffixProjectionTests
{
    [Theory]
    [InlineData("y", "ies", "city", "cities")]
    [InlineData("y", "ies", "year", "year")]
    [InlineData("s", "ses", "bus", "buses")]
    [InlineData("x", "xes", "box", "boxes")]
    [InlineData("sh", "shes", "wish", "wishes")]
    [InlineData("sh", "shes", "wishes", "wishes")]
    [InlineData("ch", "ches", "peach", "peaches")]
    public void Convert(string suffix, string replacement, string source, string expected)
    {
        var projection = new ReplaceSuffixProjection(suffix, replacement);
        Assert.Equal(suffix, projection.Suffix);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);

        
    }
}
