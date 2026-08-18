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
    //[InlineData("sh", "shes", "wishes", "wishes")]
    [InlineData("ch", "ches", "peach", "peaches")]
    public void Convert(string suffix, string replacement, string source, string expected)
    {
        IProjection<string> projection = new ReplaceSuffixProjection(suffix, replacement);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
        var reversed = projection.Reverse();
        reversed.TryConvert(expected, out var source2);
        Assert.Equal(source, source2);
    }
    [Theory]
    [InlineData("y", "ies", "city", "cities")]
    [InlineData("y", "ies", "year", "year")]
    [InlineData("s", "ses", "bus", "buses")]
    [InlineData("x", "xes", "box", "boxes")]
    [InlineData("sh", "shes", "wish", "wishes")]
    [InlineData("sh", "shes", "wishes", "wishes")]
    [InlineData("ch", "ches", "peach", "peaches")]
    public void Convert2(string suffix, string replacement, string source, string expected)
    {
        var result = ReplaceSuffixProjection.Convert(source, suffix, replacement);
        Assert.Equal(expected, result);
    }
}
