using Hand.Rule;

namespace Hand.ValidationTests;

public class IncludeRuleTests
{
    [Theory]
    [InlineData("_", "userName", false)]
    [InlineData("_", "_userName", true)]
    [InlineData("-", "userName", false)]
    [InlineData("-", "user-name", true)]
    public void Validate(string part, string argument, bool expected)
    {
        var rule = Logic.Include(part);
        Assert.Equal(expected, rule.Validate(argument));
    }
}
