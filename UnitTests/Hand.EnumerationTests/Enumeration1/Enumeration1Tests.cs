namespace Hand.EnumerationTests.Enumeration1;

public class Enumeration1Tests
{
    [Fact]
    public void Items()
    {
        var provider = CardType.Provider;
        CardType[] cardTypes = provider.Items;
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        var provider = CardType.Provider;
        // 按枚举名获取
        var silver = provider.Get("Silver");
        Assert.NotNull(silver);
        // 按别名获取
        var vip = provider.Get("Vip");
        Assert.NotNull(vip);
        Assert.Equal(CardType.Silver, silver);
        Assert.Equal(silver, vip);
    }
    [Fact]
    public void FromName_Null()
    {
        // 获取不存在的枚举时，返回 null
        var unknown = CardType.Provider.Get("");
        Assert.Null(unknown);
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
        var cardType1 = CardType.Provider.Get(1);
        Assert.Equal(CardType.Silver, cardType1);
    }
    [Fact]
    public void FromOriginal_Null()
    {
        // 获取不存在的枚举时，返回 null
        var cardType9 = CardType.Provider.Get(9);
        Assert.Null(cardType9);
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
        Assert.True(provider.IsDefined(nameof(CardType.Gold)));
        Assert.True(provider.IsDefined(CardType.Silver.Original));
        Assert.True(provider.IsDefined(CardType.Silver));
        Assert.False(provider.IsDefined("SVip"));
    }
}
