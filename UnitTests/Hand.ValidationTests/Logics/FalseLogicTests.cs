using Hand.Rule;

namespace Hand.ValidationTests.Logics;

public class FalseLogicTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("true")]
    [InlineData("fasle")]
    public void Validate(string? argument)
    {
        Assert.False(Logic.False<string>().Validate(argument!));
    }
    [Fact]
    public void Not()
    {
        var result = Logic.False<string>().Not();
        Assert.Equal(Logic.True<string>(), result);
    }
}
