using Hand.Rule.Configurations;

namespace Hand.ValidationTests.Configurations;

public class ComplexConfigurationTests
{
    [Theory]
    [MemberData(nameof(TestDataSource), DisableDiscoveryEnumeration = true)]
    public void GetConfig(TestData data)
    {
        var cfg = new ComplexConfiguration<string>(new HashSet<string>(data.Includes), new HashSet<string>(data.Excludes));
        Assert.Equal(data.Expected, cfg.GetConfig(data.Argument, data.DefaultValue));
    }


    public static TheoryData<TestData> TestDataSource =
    [
        new TestData("Name", false, false, ["Id"], "Name"),
        new TestData("Id", false, true, ["Id"], "Name"),
        new TestData("Name", true, false, ["Id"], "Name"),
        new TestData("Id", true, true, ["Id"], "Name"),
        new TestData("Id", false, false, ["Name"], "Id"),
        new TestData("Name", false, true, ["Name"], "Id"),
        new TestData("Id", true, false, ["Name"], "Id"),
        new TestData("Name", true, true, ["Name"], "Id")
    ];
    public record TestData(string Argument, bool DefaultValue, bool Expected, string[] Includes, params string[] Excludes);
}
