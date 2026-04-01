using Hand.Paths;

namespace NamingTests.Paths;

public class PascalSplitRuleTests
{
    [Theory]
    [InlineData('_', false)]
    [InlineData('-', false)]
    [InlineData('a', false)]
    [InlineData('A', true)]
    [InlineData('1', false)]
    public void Validate(char item, bool expected)
    {
        var actual = PascalSplitRule.Validation.Validate(item);
        Assert.Equal(expected, actual);
    }
}
