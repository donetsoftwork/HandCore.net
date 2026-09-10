using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enum1;

public class Enum1Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        Enumeration[] cardTypes = provider.Items;
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        // 按枚举名获取
        Enumeration? cardType = provider.Get(nameof(CardType.Silver));
        Assert.NotNull(cardType);
        Assert.Equal(nameof(CardType.Silver), cardType.Name);
        Assert.Empty(cardType.Description);
    }
    [Fact]
    public void FromOriginal()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        // 按枚举值获取
        Enumeration? cardType = provider.Get(2);
        Assert.NotNull(cardType);
        Assert.Equal(nameof(CardType.Gold), cardType.Name);
        Assert.Empty(cardType.Description);
    }
    [Fact]
    public void FromEnum()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        Enumeration? cardType = provider.Get((long)CardType.Silver);
        Assert.NotNull(cardType);
        Assert.Equal(nameof(CardType.Silver), cardType.Name);
        Assert.Empty(cardType.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        Assert.True(provider.IsDefined(nameof(CardType.Gold)));
        Assert.True(provider.IsDefined((long)CardType.Silver));
        Assert.False(provider.IsDefined("SVip"));
        var other = new Enumeration("SVip", 100L, "超级会员");
        Assert.False(provider.IsDefined(other));
    }
}
