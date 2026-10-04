using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enumeration3;

public class Enumeration3Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        Enumeration[] cardTypes = [.. provider.Items];
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 按枚举名获取
        Assert.True(provider.TryGet("Silver", out var silver));
        // 按别名获取
        Assert.True(provider.TryGet("Vip", out var vip));
        Assert.NotNull(vip);
        Assert.Equal(CardType.Silver, silver);
        Assert.Equal(silver, vip);
    }
    [Fact]
    public void FromName_Null()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 获取不存在的枚举时，返回 null
        Assert.False(provider.TryGet("", out _));
    }
    [Fact]
    public void FromName_Default()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 获取不存在的枚举时指定默认值，返回默认值
        var unknownDefault = provider.Get("", CardType.Silver);
        Assert.Equal(CardType.Silver, unknownDefault);
    }
    [Fact]
    public void FromOriginal()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 按枚举值获取
        Assert.True(provider.TryGet(1, out var cardType1));
        Assert.Equal(CardType.Silver, cardType1);
    }
    [Fact]
    public void FromOriginal_Null()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 获取不存在的枚举时，返回 null
        Assert.False(provider.TryGet(9, out _));
    }
    [Fact]
    public void FromOriginal_Default()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        // 获取不存在的枚举时指定默认值，返回默认值
        var cardType0Default = provider.Get(0, CardType.Silver);
        Assert.Equal(CardType.Silver, cardType0Default);
    }
    [Fact]
    public void IsDefined()
    {
        IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
        Assert.True(provider.IsDefined(nameof(CardType.Gold)));
        Assert.True(provider.IsDefined(CardType.Silver.Original));
        Assert.True(provider.IsDefined(CardType.Silver));
        Assert.False(provider.IsDefined("SVip"));
    }
    [Fact]
    public void Provider()
    {
        var provider = CardType.Provider;
        Enumeration[] cardTypes = [.. provider.Items];
        Assert.Equal(4, cardTypes.Length);
    }
}
