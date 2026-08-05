using Hand.Words;

namespace NamingTests.Words;

public class CamelWordRuleTests
{
    [Theory]
    [InlineData("Test", "test")]
    [InlineData("_id", "id")]
    [InlineData("", "")]
    public void FistToLower(string original, string expected)
    {
        var result = CamelWordRule.FistToLower(original!);
        Assert.Equal(expected, result);
    }
}
