using Hand.Enumerations;

namespace Hand.EnumerationTests;

public class FlagEnumerationTests
{
    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    [InlineData(6, false)]
    [InlineData(7, false)]
    [InlineData(8, true)]
    public void VerifyFlag(long original, bool expected)
    {
        var result = FlagEnumeration.VerifyFlag(original);
        Assert.Equal(expected, result);
    }
}
