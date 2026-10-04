using Hand.Rule.Configurations;

namespace Hand.ValidationTests.Configurations;

public class ExcludeConfigurationTests
{
    [Theory]
    [InlineData("Name", false, false, "Id")]
    [InlineData("Name", true, true, "Id")]
    [InlineData("Id", false, false, "Name")]
    [InlineData("Id", true, true, "Name")]
    [InlineData("Id", false, false, "Id", "Name")]
    [InlineData("Id", true, false, "Id", "Name")]
    [InlineData("Name", false, false, "Id", "Name")]
    [InlineData("Name", true, false, "Id", "Name")]
    public void GetConfig(string argument, bool defaultValue, bool expected, params string[] members)
    {
        var cfg = new ExcludeConfiguration<string>(new HashSet<string>(members));
        Assert.Equal(expected, cfg.GetConfig(argument, defaultValue));
    }
}
