using Hand.Words;

namespace NamingTests.Words;

public class PascalWordRuleTests
{
    [Theory]
    [InlineData("test", "Test")]
    [InlineData("_id", "_id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void FistToUpper(string? original, string? expected)
    {
        var result = PascalWordRule.FistToUpper(original!);
        Assert.Equal(expected, result);
    }
}
