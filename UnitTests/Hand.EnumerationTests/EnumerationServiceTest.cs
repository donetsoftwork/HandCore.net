using Hand.Enumerations;

namespace Hand.EnumerationTests;

public class EnumerationServiceTest
{
    [Fact]
    public void GetAll()
    {
        var cardTypes = EnumerationService.GetAll<CardType>()
            .ToList();
        Assert.Equal(4, cardTypes.Count);
    }
    [Fact]
    public void FromValue()
    {
        var cardType = EnumerationService.FromValue<CardType>(2);
        Assert.Equal(CardType.Gold, cardType);
    }
    [Fact]
    public void FromDisplay()
    {
        var cardType = EnumerationService.FromDisplay<CardType>("铂金卡");
        Assert.Equal(CardType.Platinum, cardType);
    }
    [Fact]
    public void FromCode()
    {
        var cardType = EnumerationService.FromCode<CardType>("Black");
        Assert.Equal(CardType.Black, cardType);
    }
}
/// <summary>
/// Vip卡类型
/// </summary>
/// <param name="code"></param>
/// <param name="value"></param>
/// <param name="display"></param>
public class CardType(string code, int value, string display)
    : EnumerationBase(code, value, display)
{
    public static readonly CardType Silver = new(nameof(Silver), 1, "白银卡");
    public static readonly CardType Gold = new(nameof(Gold), 2, "黄金卡");
    public static readonly CardType Platinum = new(nameof(Platinum), 3, "铂金卡");
    public static readonly CardType Black = new(nameof(Black), 4, "黑金卡");
}