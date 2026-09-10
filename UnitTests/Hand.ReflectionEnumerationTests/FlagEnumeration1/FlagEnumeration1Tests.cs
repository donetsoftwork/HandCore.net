using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.FlagEnumeration1;

public class FlagEnumeration1Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        DaysOfWeek[] cardTypes = provider.Items;
        Assert.Equal(7, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        // 按枚举名获取
        FlagEnumeration? monday = provider.Get(nameof(DaysOfWeek.Monday));
        Assert.NotNull(monday);
        Assert.Equal(nameof(DaysOfWeek.Monday), monday.Name);
        Assert.Equal("星期一", monday.Description);
    }
    [Fact]
    public void FromOriginal()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        // 按枚举值获取
        FlagEnumeration? day1 = provider.Get(1);
        Assert.NotNull(day1);
        Assert.Equal(nameof(DaysOfWeek.Sunday), day1.Name);
        Assert.Equal("星期日", day1.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        Assert.True(provider.IsDefined(DaysOfWeek.Saturday));
        Assert.True(provider.IsDefined(nameof(DaysOfWeek.Saturday)));
        Assert.True(provider.IsDefined(DaysOfWeek.Saturday.Original));
        Assert.False(provider.IsDefined("DayEight"));
        var other = new DaysOfWeek(StringComparer.Ordinal, "DayEight", 1 << 7, "星期八");
        Assert.False(provider.IsDefined(other));
    }
    [Fact]
    public void Or()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        var weekend = provider.Or(DaysOfWeek.Saturday, DaysOfWeek.Sunday);
        Assert.True(weekend.HasFlag(DaysOfWeek.Saturday));
        Assert.False(weekend.HasFlag(DaysOfWeek.Monday));
    }
    [Fact]
    public void And()
    {
        IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
        // 假设员工一周一和周二值班
        var duty1 = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday);
        // 假设员工二周二和周三值班
        var duty2 = provider.Or(DaysOfWeek.Tuesday, DaysOfWeek.Wednesday);
        /// 获取两员工共同值班的时间
        var overlay = provider.And(duty1, duty2);
        Assert.Equal(DaysOfWeek.Tuesday, overlay);
    }
}
