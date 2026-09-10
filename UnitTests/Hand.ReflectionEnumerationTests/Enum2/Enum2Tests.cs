using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enum2;

public class Enum2Tests
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
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>(StringComparer.OrdinalIgnoreCase);
        // 按枚举名获取
        Enumeration? silver = provider.Get(nameof(CardType.Silver));
        Assert.NotNull(silver);
        Assert.Equal(nameof(CardType.Silver), silver.Name);
        Assert.Equal("银卡", silver.Description);
        // 按枚举别名获取
        Enumeration? vip = provider.Get("vip");
        Assert.NotNull(vip);
        Assert.Equal(silver, vip);
    }
    [Fact]
    public void FromOriginal()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        // 按枚举值获取
        Enumeration? gold = provider.Get(2);
        Assert.NotNull(gold);
        Assert.Equal(nameof(CardType.Gold), gold.Name);
        Assert.Equal("金卡", gold.Description);
    }
    [Fact]
    public void FromEnum()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        Enumeration? cardType = provider.Get((long)CardType.Silver);
        Assert.NotNull(cardType);
        Assert.Equal(nameof(CardType.Silver), cardType.Name);
        Assert.Equal("银卡", cardType.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>(StringComparer.OrdinalIgnoreCase);
        Assert.True(provider.IsDefined(nameof(CardType.Gold)));
        Assert.True(provider.IsDefined(nameof(CardType.Vip)));
        Assert.True(provider.IsDefined("svip"));
        var other = new Enumeration("vvip", 100L, "超级会员");
        Assert.False(provider.IsDefined(other));
    }
    [Fact]
    public void EnumMember()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>(StringComparer.OrdinalIgnoreCase);
        // 用EnumMember标记定义别名
        Enumeration? svip = provider.Get("svip");
        Assert.NotNull(svip);
    }
    [Fact]
    public void Description()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<CardType>();
        // 支持用Description标记定义备注
        Enumeration? description = provider.GetByDescription("银卡");
        Assert.NotNull(description);
    }
}
