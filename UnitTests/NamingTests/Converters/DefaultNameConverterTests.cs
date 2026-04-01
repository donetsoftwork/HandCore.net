using Hand.Converters;

namespace NamingTests.Converters;

public class DefaultNameConverterTests
{
    [Theory]
    [InlineData("abc", "abc")]
    [InlineData("Def", "Def")]
    [InlineData(null, null)]
    [InlineData("", "")]
    public void Validate(string? item, string? expected)
    {
        var actual = DefaultNameConverter.Instance.Convert(item);
        Assert.Equal(expected, actual);
    }
}
