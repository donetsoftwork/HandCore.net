using Hand.Enumerations;

namespace Hand.EnumerationTests.Enumeration3;

public class Enumeration3Tests
{
    [Fact]
    public void Items()
    {
        var provider = CardType.Provider;
        Enumeration[] cardTypes = provider.Items;
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        var provider = CardType.Provider;
        // 按枚举名获取
        var cardType = provider.Get("Silver");
        Assert.NotNull(cardType);
        // 按别名获取
        var vip = provider.Get("Vip");
        Assert.NotNull(vip);
        Assert.Equal(CardType.Silver, cardType);
        Assert.Equal(cardType, vip);
    }
    [Fact]
    public void FromName_Null()
    {
        // 获取不存在的枚举时，返回 null
        var cardType = CardType.Provider.Get("");
        Assert.Null(cardType);
    }
    [Fact]
    public void FromName_Default()
    {
        var defaultValue = CardType.Silver;
        // 获取不存在的枚举时指定默认值，返回默认值
        var cardType = CardType.Provider.Get("", defaultValue);
        Assert.Equal(defaultValue, cardType);
    }
    [Fact]
    public void FromOriginal()
    {
        // 按枚举值获取
        var cardType = CardType.Provider.Get(1);
        Assert.Equal(CardType.Silver, cardType);
    }
    [Fact]
    public void FromOriginal_Null()
    {
        // 获取不存在的枚举时，返回 null
        var cardType = CardType.Provider.Get(9);
        Assert.Null(cardType);
    }
    [Fact]
    public void FromOriginal_Default()
    {
        var defaultValue = CardType.Silver;
        // 获取不存在的枚举时指定默认值，返回默认值
        var cardType = CardType.Provider.Get(0, defaultValue);
        Assert.Equal(defaultValue, cardType);
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
