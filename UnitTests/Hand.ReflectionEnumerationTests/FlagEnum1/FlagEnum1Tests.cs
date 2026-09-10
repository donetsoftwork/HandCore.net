using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.FlagEnum1;

public class FlagEnum1Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        Assert.Equal(7, provider.Items.Length);
        Assert.Equal(7, provider.Flags.Length);
    }
    [Fact]
    public void FromName()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        // 按枚举名获取
        FlagEnumeration? monday = provider.Get(nameof(DaysOfWeek.Monday));
        Assert.NotNull(monday);
        Assert.Equal(nameof(DaysOfWeek.Monday), monday.Name);
        Assert.Empty(monday.Description);
    }
    [Fact]
    public void FromOriginal()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        // 按枚举值获取
        FlagEnumeration? day1 = provider.Get(1);
        Assert.NotNull(day1);
        Assert.Equal(nameof(DaysOfWeek.Sunday), day1.Name);
        Assert.Empty(day1.Description);
    }
    [Fact]
    public void FromEnum()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        FlagEnumeration? friday = provider.Get((long)DaysOfWeek.Friday);
        Assert.NotNull(friday);
        Assert.Equal(nameof(DaysOfWeek.Friday), friday.Name);
        Assert.Empty(friday.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        Assert.True(provider.IsDefined(nameof(DaysOfWeek.Monday)));
        Assert.True(provider.IsDefined((long)DaysOfWeek.Saturday));
        Assert.False(provider.IsDefined("DayEight"));
        var other = new FlagEnumeration(StringComparer.Ordinal, "DayEight", 1 << 7, "星期八");
        Assert.False(provider.IsDefined(other));
    }
    [Fact]
    public void Empty()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        var empty = provider.Empty;
        Assert.Equal(0L, empty.Original);
    }
    [Fact]
    public void TryParseByName()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        Assert.True(provider.TryParse(nameof(DaysOfWeek.Monday), out var monday));
        Assert.Equal(nameof(DaysOfWeek.Monday), monday.Name);
        Assert.True(provider.TryParse("Saturday,Sunday", out var weekend));
        Assert.True(weekend.HasFlag(nameof(DaysOfWeek.Saturday)));
        Assert.True(weekend.HasFlag((long)DaysOfWeek.Sunday));
    }
    [Fact]
    public void TryParseByOriginal()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<DaysOfWeek>();
        Assert.True(provider.TryParse((long)DaysOfWeek.Monday, out var monday));
        Assert.Equal(nameof(DaysOfWeek.Monday), monday.Name);
        Assert.True(provider.TryParse((long)(DaysOfWeek.Saturday | DaysOfWeek.Sunday), out var weekend));
        Assert.True(weekend.HasFlag(nameof(DaysOfWeek.Saturday)));
        Assert.True(weekend.HasFlag((long)DaysOfWeek.Sunday));
    }
}
