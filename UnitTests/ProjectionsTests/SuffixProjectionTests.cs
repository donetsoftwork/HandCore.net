using Hand.Maping;

namespace ProjectionsTests;

public class SuffixProjectionTests
{
    [Theory]
    [InlineData("s", "book", "books")]
    [InlineData("es", "bus", "buses")]
    public void Convert(string suffix, string source, string expected)
    {
        IProjection<string> projection = new SuffixProjection(suffix);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
        var reversed = projection.Reverse();
        reversed.TryConvert(expected, out var source2);
        Assert.Equal(source, source2);
    }
}
