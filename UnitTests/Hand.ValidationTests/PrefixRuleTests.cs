using Hand.Rule;

namespace Hand.ValidationTests;

public class PrefixRuleTests
{
    [Theory]
    [InlineData("User", "Id", false)]
    [InlineData("User", "UserName", true)]
    public void Convert(string prefix, string source, bool expected)
    {
        var validation = Logic.Prefix(prefix);
        Assert.Equal(expected, validation.Validate(source));
    }
}
