using Hand.Rule;

namespace NamingTests.Paths;

public class DefaultSplitRuleTests
{
    private readonly IValidation<char> _rule = Logic.Included(['_', '-']);

    [Theory]
    [InlineData('_', true)]
    [InlineData('-', true)]
    [InlineData('a', false)]
    [InlineData('A', false)]
    [InlineData('1', false)]
    public void Validate(char item, bool expected)
    {
        var actual = _rule.Validate(item);
        Assert.Equal(expected, actual);
    }
}
