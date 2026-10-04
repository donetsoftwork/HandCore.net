using Hand.Enumerations;

namespace Hand.EnumerationTests.Enum1;

public class Enum1Tests
{
    [Fact]
    public void GetEnumprovider()
    {
        IEnumerationProvider<Enumeration> provider = CardTypeProvider.Instance;
        Enumeration[] cardTypes = [.. provider.Items];
        Assert.Equal(4, cardTypes.Length);
    }
    [Fact]
    public void FromName()
    {
        IEnumerationProvider<Enumeration> provider = CardTypeProvider.Instance;
        // 按枚举名获取
        Assert.True(provider.TryGet(nameof(CardType.Silver), out var cardType));
        Assert.Equal(nameof(CardType.Silver), cardType.Name);
        Assert.Equal("银卡", cardType.Description);
    }
    [Fact]
    public void FromOriginal()
    {
        IEnumerationProvider<Enumeration> provider = CardTypeProvider.Instance;
        // 按枚举值获取
        Assert.True(provider.TryGet(2, out var cardType));
        Assert.Equal(nameof(CardType.Gold), cardType.Name);
        Assert.Equal("金卡", cardType.Description);
    }
    [Fact]
    public void FromEnum()
    {
        IEnumerationProvider<Enumeration> provider = CardTypeProvider.Instance;
        Assert.True(provider.TryGet((long)CardType.Silver, out var cardType));
        Assert.Equal(nameof(CardType.Silver), cardType.Name);
        Assert.Equal("银卡", cardType.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IEnumerationProvider<Enumeration> provider = CardTypeProvider.Instance;
        Assert.True(provider.IsDefined(nameof(CardType.Gold)));
        Assert.True(provider.IsDefined((long)CardType.Silver));
        Assert.False(provider.IsDefined("SVip"));
        var other = new Enumeration("SVip", 100L, "超级会员");
        Assert.False(provider.IsDefined(other));
    }
}
