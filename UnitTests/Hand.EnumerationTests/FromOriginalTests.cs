using Hand.EnumerationTests.Enumeration2;
using Hand.EnumerationTests.Supports;

namespace Hand.EnumerationTests;

public class FromOriginalTests
{
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
    public void FromOriginal_Default2()
    {
        var defaultValue = DaysOfWeekEnumeration.Monday;
        // 建议指定默认值比内部默认值优先级更好
        // 0是Unknown的原始值，这里指定默认值为 Monday，返回 Monday
        var daysOfWeek = DaysOfWeekEnumeration.Provider.Instance.Get(0, defaultValue);
        Assert.Equal(defaultValue, daysOfWeek);
    }
}
