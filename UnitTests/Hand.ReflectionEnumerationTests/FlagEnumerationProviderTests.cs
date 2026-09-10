using Hand.Enumerations;
using Hand.Primitives;
using Hand.ReflectionEnumerationTests.Supports;

namespace Hand.ReflectionEnumerationTests;

public class FlagEnumerationProviderTests
{
    [Fact]
    public void GetFlagEnumProvider()
    {
        var provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        Assert.Equal(7, provider.Count);
    }
    [Fact]
    public void FromName()
    {
        var provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        var weekName = "Monday";
        var monday = provider.Get(weekName);
        Assert.NotNull(monday);
        Assert.Equal(weekName, monday.Name);
    }
    [Fact]
    public void FromOriginal()
    {
        var provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        var monday = provider.Get(2);
        Assert.NotNull(monday);
        Assert.Equal("Monday", monday.Name);
    }
    [Fact]
    public void FromDescription()
    {
        var provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        var monday = provider.GetByDescription("周一");
        Assert.NotNull(monday);
        Assert.Equal("Monday", monday.Name);
    }
    [Fact]
    public void Or()
    {
        var provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        var saturday = provider.Get(1);
    }
}
