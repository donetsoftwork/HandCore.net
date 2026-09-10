using Hand.Enumerations;
using Hand.EnumerationTests.Supports;

namespace Hand.EnumerationTests;

public class DaysOfWeekEnumerationTests
{
    [Fact]
    public void FromName()
    {
        var daysOfWeek = DaysOfWeekEnumeration.Provider.Instance.Get("Sunday");
        Assert.NotNull(daysOfWeek);
        Assert.True(EnumerationComparer<DaysOfWeekEnumeration>.Equals(DaysOfWeekEnumeration.Sunday, daysOfWeek));
    }
    [Fact]
    public void FromOriginal()
    {
        var daysOfWeek = DaysOfWeekEnumeration.Provider.Instance.Get(1);
        Assert.NotNull(daysOfWeek);
        Assert.True(EnumerationComparer<DaysOfWeekEnumeration>.Equals(DaysOfWeekEnumeration.Sunday, daysOfWeek));
    }
    [Fact]
    public void Or()
    {
        var provider = DaysOfWeekEnumeration.Provider.Instance;
        var weekend = provider.Or(DaysOfWeekEnumeration.Saturday, DaysOfWeekEnumeration.Sunday, "周末");
        // 使用HasFlag判断是否为周末
        Assert.True(weekend.HasFlag(DaysOfWeekEnumeration.Saturday));
        Assert.True(weekend.HasFlag(DaysOfWeekEnumeration.Sunday));
        Assert.False(weekend.HasFlag(DaysOfWeekEnumeration.Monday));
    }
    [Fact]
    public void And()
    {
        var provider = DaysOfWeekEnumeration.Provider.Instance;
        var weekend = provider.Or(DaysOfWeekEnumeration.Saturday, DaysOfWeekEnumeration.Sunday, "周末");
        var saturday = provider.And(weekend, DaysOfWeekEnumeration.Sunday);
        // 使用IsUnknown判断是否为周末
        Assert.False(saturday.IsUnknown);
        var monday = provider.And(weekend, DaysOfWeekEnumeration.Monday);
        Assert.True(monday.IsUnknown);
    }
}
