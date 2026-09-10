using Hand.Enumerations;
using Hand.ReflectionEnumerationTests.Supports;

namespace Hand.ReflectionEnumerationTests;

public class EnumproviderTests
{
    [Fact]
    public void GetEnumprovider()
    {
        var provider = ReflectionEnumeration.GetEnumProvider<CardEnum>();
        Assert.Equal(4, provider.Count);
    }
    [Fact]
    public void GetEnumerationprovider()
    {
        var provider = ReflectionEnumeration.GetEnumerationProvider<CardEnumeration>();
        Assert.Equal(5, provider.Count);
    }
    [Fact]
    public void FromName()
    {
        var provider = ReflectionEnumeration.GetEnumProvider<CardEnum>();
        var cardName = "Silver";
        // 按枚举名获取
        var silver = provider.Get(cardName);
        Assert.NotNull(silver);
        Assert.Equal(cardName, silver.Name);
        // 按枚举别名获取
        var vip = provider.Get("Vip");
        Assert.NotNull(vip);
        Assert.Equal(silver, vip);
    }
    [Fact]
    public void FromOriginal()
    {
        var provider = ReflectionEnumeration.GetEnumerationProvider<CardEnumeration>();
        var cardEnumeration = provider.Get(2);
        Assert.NotNull(cardEnumeration);
        Assert.Equal(CardEnumeration.Gold, cardEnumeration);
    }
    [Fact]
    public void FromEnum()
    {
        var provider = ReflectionEnumeration.GetEnumProvider<CardEnum>();
        var cardEnumeration = provider.Get((long)CardEnum.Silver);
        Assert.NotNull(cardEnumeration);
        Assert.Equal(nameof(CardEnum.Silver), cardEnumeration.Name);
    }
    [Fact]
    public void FromDescription()
    {
        var provider = ReflectionEnumeration.GetEnumerationProvider<CardEnumeration>();
        var cardEnumeration = provider.GetByDescription("铂金卡");
        Assert.NotNull(cardEnumeration);
        Assert.Equal(CardEnumeration.Platinum, cardEnumeration);
    }
}
