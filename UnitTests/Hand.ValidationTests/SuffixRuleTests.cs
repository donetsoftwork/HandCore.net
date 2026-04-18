using Hand.Rule;

namespace Hand.ValidationTests;

public class SuffixRuleTests
{
    [Theory]
    [InlineData("s", "User", false)]
    [InlineData("es", "buses", true)]
    public void Convert(string suffix, string source, bool expected)
    {
        var validation = Logic.Suffix(suffix);
        Assert.Equal(expected, validation.Validate(source));
    }
}
