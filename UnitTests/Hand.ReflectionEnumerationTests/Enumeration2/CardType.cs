using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enumeration2;

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

    /// <inheritdoc cref="Silver" path="/summary"/>
    public static CardType Vip => Silver;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public static CardType Vip2 => Gold;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public static CardType Vip3 => Platinum;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static CardType Vip4 => Black;
}