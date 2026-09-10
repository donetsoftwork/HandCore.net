using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enumeration1;

/// <summary>
/// Vip卡类型
/// </summary>
public sealed class CardType : Enumeration
{
    private CardType(string name, long original, string description = "")
        : base(name, original, description)
    {
    }
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly CardType Silver = new(nameof(Silver), 1, "银卡");
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly CardType Gold = new(nameof(Gold), 2, "金卡");
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly CardType Platinum = new(nameof(Platinum), 3, "铂金卡");
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly CardType Black = new(nameof(Black), 4, "黑金卡");
}