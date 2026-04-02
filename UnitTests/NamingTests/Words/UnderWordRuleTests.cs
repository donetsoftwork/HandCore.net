using Hand.Words;

namespace NamingTests.Words;

public class UnderWordRuleTests
{
    [Theory]
    [InlineData("test", "_test")]
    [InlineData("_test", "_test")]
    [InlineData(null, "_")]
    [InlineData("", "_")]
    public void Under(string? original, string? expected)
    {
        var result = UnderWordRule.Under(original!);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData("Test", "_test")]
    [InlineData("test", "_test")]
    [InlineData("_test", "_test")]
    [InlineData("_Test", "_test")]
    [InlineData(null, "_")]
    [InlineData("", "_")]
    public void UnderLower(string? original, string? expected)
    {
        var result = UnderWordRule.UnderLower(original!);
        Assert.Equal(expected, result);
    }
}
