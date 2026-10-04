using Hand.Enumerations;

namespace Hand.EnumerationTests.Enumeration2;

public class Enumeration2Tests
{
    [Fact]
    public void Items()
    {
        IEnumerationProvider<CardType> provider = CardType.Provider;
        CardType[] cardTypes = [.. provider.Items];
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        var provider = CardType.Provider;
        // 按枚举名获取
        Assert.True(provider.TryGet("silver", out var silver));
        // 按别名获取
        Assert.True(provider.TryGet("vip", out var vip));
        Assert.Equal(CardType.Silver, silver);
        Assert.Equal(silver, vip);
    }
    [Fact]
    public void FromName_Null()
    {
        // 获取不存在的枚举时，返回 null
        Assert.False(CardType.Provider.TryGet("", out _));
    }
    [Fact]
    public void FromName_Default()
    {
        // 获取不存在的枚举时指定默认值，返回默认值
        var unknownDefault = CardType.Provider.Get("", CardType.Silver);
        Assert.Equal(CardType.Silver, unknownDefault);
    }
    [Fact]
    public void FromOriginal()
    {
        // 按枚举值获取
        Assert.True(CardType.Provider.TryGet(1, out var cardType1));
        Assert.Equal(CardType.Silver, cardType1);
    }
    [Fact]
    public void FromOriginal_Null()
    {
        // 获取不存在的枚举时，返回 null
        Assert.False(CardType.Provider.TryGet(9, out _));
    }
    [Fact]
    public void FromOriginal_Default()
    {
        // 获取不存在的枚举时指定默认值，返回默认值
        var cardType0Default = CardType.Provider.Get(0, CardType.Silver);
        Assert.Equal(CardType.Silver, cardType0Default);
    }
    [Fact]
    public void IsDefined()
    {
        var provider = CardType.Provider;
        Assert.True(provider.IsDefined("gold"));
        Assert.True(provider.IsDefined(CardType.Silver.Original));
        Assert.True(provider.IsDefined(CardType.Silver));
        Assert.False(provider.IsDefined("SVip"));
    }
}
