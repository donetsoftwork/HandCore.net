using Hand.Maping;

namespace ProjectionsTests;

public class SuffixProjectionTests
{
    [Theory]
    [InlineData("s", "book", "books")]
    [InlineData("es", "bus", "buses")]
    public void Convert(string suffix, string source, string expected)
    {
        var projection = new SuffixProjection(suffix);
        Assert.Equal(suffix, projection.Suffix);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
