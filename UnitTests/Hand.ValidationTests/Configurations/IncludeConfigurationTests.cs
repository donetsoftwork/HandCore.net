using Hand.Rule.Configurations;

namespace Hand.ValidationTests.Configurations;

public class IncludeConfigurationTests
{
    [Theory]
    [InlineData("Name", false, false, "Id")]
    [InlineData("Name", true, true, "Id")]
    [InlineData("Id", false, false, "Name")]
    [InlineData("Id", true, true, "Name")]
    [InlineData("Id", false, true, "Id", "Name")]
    [InlineData("Id", true, true, "Id", "Name")]
    [InlineData("Name", false, true, "Id", "Name")]
    [InlineData("Name", true, true, "Id", "Name")]
    public void GetConfig(string argument, bool defaultValue, bool expected, params string[] members)
    {
        var cfg = new IncludeConfiguration<string>(new HashSet<string>(members));
        Assert.Equal(expected, cfg.GetConfig(argument, defaultValue));
    }
}
