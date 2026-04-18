using Hand.Rule;

namespace Hand.ValidationTests.Logics;

public class TrueLogicTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("true")]
    [InlineData("fasle")]
    public void Validate(string? argument)
    {
        Assert.True(Logic.True<string>().Validate(argument!));
    }
    [Fact]
    public void Not()
    {
        var result = Logic.True<string>().Not();
        Assert.Equal(Logic.False<string>(), result);
    }
    [Fact]
    public void And()
    {
        Assert.Equal(Logic.True<string>(), Logic.True<string>().And(Logic.True<string>()));
        Assert.Equal(Logic.False<string>(), Logic.True<string>().And(Logic.False<string>()));
    }
    [Fact]
    public void Or()
    {
        var result = Logic.True<string>().Or(Logic.False<string>());
        Assert.Equal(Logic.True<string>(), result);
    }
}
