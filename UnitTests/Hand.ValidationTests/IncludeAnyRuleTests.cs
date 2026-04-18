using Hand.Rule;

namespace Hand.ValidationTests;

public class IncludeAnyRuleTests
{    
    [Theory]
    [InlineData('_', "userName", false)]
    [InlineData('_', "_userName", true)]
    [MemberData(nameof(ValidateData))]
    public void Validate(char part, string argument, bool expected)
    {
        var rule = Logic.IncludeAny(part);
        Assert.Equal(expected, rule.Validate(argument));
    }

    public static IEnumerable<object[]> ValidateData =>
    [
        ['-', "userName", false],
        ['-', "user-name", true]
    ];
}
