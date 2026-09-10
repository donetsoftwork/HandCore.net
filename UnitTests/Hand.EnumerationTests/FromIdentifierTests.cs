using Hand.EnumerationTests.Enumeration2;
using Hand.EnumerationTests.Supports;

namespace Hand.EnumerationTests;

public class FromNameTests
{
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
    public void FromName_Default2()
    {
        var defaultValue = DaysOfWeekEnumeration.Monday;
        // 建议指定默认值比内部默认值优先级更好
        // 这里指定默认值为 Monday，返回 Monday不返回Unknown
        var daysOfWeek = DaysOfWeekEnumeration.Provider.Instance.Get("Unknown", defaultValue);
        Assert.Equal(defaultValue, daysOfWeek);
    }
    //public class CardType2()
    //    : CardType("Silver", 1, "银卡")
    //{
    //    /// <inheritdoc />
    //    public override decimal Discount => 0.1m;
    //}
}
