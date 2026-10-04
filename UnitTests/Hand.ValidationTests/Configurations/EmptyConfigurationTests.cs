using Hand.Rule.Configurations;

namespace Hand.ValidationTests.Configurations;

public class EmptyConfigurationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("true")]
    [InlineData("fasle")]
    public void Validate(string? argument)
    {
        Assert.False(EmptyConfiguration<string>.Instance.Validate(argument!));
    }
}
