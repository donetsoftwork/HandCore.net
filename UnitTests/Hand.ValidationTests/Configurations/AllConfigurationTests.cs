using Hand.Rule.Configurations;

namespace Hand.ValidationTests.Configurations;

public class AllConfigurationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("true")]
    [InlineData("fasle")]
    public void Validate(string? argument)
    {
        Assert.True(AllConfiguration<string>.Instance.Validate(argument!));
    }
}
