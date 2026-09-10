using Hand.Enumerations;

namespace Hand.EnumerationTests.FlagEnumeration1;

public class FlagEnumeration1Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        var provider = DaysOfWeek.Provider;
        Assert.Equal(8, provider.Items.Length);
        Assert.Equal(7, provider.Flags.Length);
    }
    [Fact]
    public void FromName()
    {
        var provider = DaysOfWeek.Provider;
        // 按枚举名获取
        FlagEnumeration? monday = provider.Get(nameof(DaysOfWeek.Monday));
        Assert.NotNull(monday);
        Assert.Equal(nameof(DaysOfWeek.Monday), monday.Name);
        Assert.Equal("星期一", monday.Description);
    }
    [Fact]
    public void FromOriginal()
    {
        var provider = DaysOfWeek.Provider;
        // 按枚举值获取
        FlagEnumeration? day1 = provider.Get(1);
        Assert.NotNull(day1);
        Assert.Equal(nameof(DaysOfWeek.Sunday), day1.Name);
        Assert.Equal("星期日", day1.Description);
    }
    [Fact]
    public void IsDefined()
    {
        var provider = DaysOfWeek.Provider;
        Assert.True(provider.IsDefined(DaysOfWeek.Saturday));
        Assert.True(provider.IsDefined(nameof(DaysOfWeek.Saturday)));
        Assert.True(provider.IsDefined(DaysOfWeek.Saturday.Original));
        Assert.False(provider.IsDefined("DayEight"));
    }
    [Fact]
    public void Or()
    {
        var provider = DaysOfWeek.Provider;
        var weekend = provider.Or(DaysOfWeek.Saturday, DaysOfWeek.Sunday);
        Assert.NotNull(weekend);
        Assert.True(weekend.HasFlag(DaysOfWeek.Sunday));
        Assert.False(weekend.HasFlag(DaysOfWeek.Monday));
    }
    [Fact]
    public void Or2()
    {
        var provider = DaysOfWeek.Provider;
        var weekday = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday);
        Assert.NotNull(weekday);
        Assert.True(weekday.HasFlag(DaysOfWeek.Monday));
        Assert.False(weekday.HasFlag(DaysOfWeek.Sunday));
    }
    [Fact]
    public void And()
    {
        var provider = DaysOfWeek.Provider;
        // 假设员工一周一和周二值班
        var duty1 = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday);
        // 假设员工二周二和周三值班
        var duty2 = provider.Or(DaysOfWeek.Tuesday, DaysOfWeek.Wednesday);
        /// 获取两员工共同值班的时间
        var overlay = provider.And(duty1, duty2);
        Assert.Equal(DaysOfWeek.Tuesday, overlay);
    }
    [Fact]
    public void And2()
    {
        var provider = DaysOfWeek.Provider;
        // 假设员工1周一和周四值班
        var duty1 = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday);
        // 假设员工2周二和周四值班
        var duty2 = provider.Or(DaysOfWeek.Tuesday, DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday);
        // 假设员工3周二和周三值班
        var duty3 = provider.Or(DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday, DaysOfWeek.Saturday);
        /// 获取三员工共同值班的时间
        var overlay = provider.And(duty1, duty2, duty3);
        Assert.True(overlay.HasFlag(DaysOfWeek.Wednesday));
        Assert.False(overlay.HasFlag(DaysOfWeek.Tuesday));
    }
}
