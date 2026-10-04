using Hand.Enumerations;

namespace Hand.EnumerationTests.Enum1;

/// <summary>
/// 源生成器样板代码
/// </summary>
public class CardTypeProvider : EnumerationProvider<Enumeration>
{
    private CardTypeProvider()
        : base(
            new Dictionary<string, Enumeration>
            {
                { nameof(Silver), Silver},
                { nameof(Gold), Gold},
                { nameof(Platinum), Platinum},
                { nameof(Black), Black},
                { nameof(Vip) , Silver},
                { nameof(Vip2) , Gold},
                { nameof(Vip3) , Platinum},
                { nameof(Vip4) , Black},
                { nameof(SVIP) , Black},
            }
    )
    {
    }
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly Enumeration Silver = new(nameof(Silver), 1, "银卡");
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly Enumeration Gold = new(nameof(Gold), 2, "金卡");
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly Enumeration Platinum = new(nameof(Platinum), 3, "铂金卡");
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly Enumeration Black = new(nameof(Black), 4, "黑金卡");

    /// <inheritdoc cref="Silver" path="/summary"/>
    public static Enumeration Vip => Silver;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public static Enumeration Vip2 => Gold;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public static Enumeration Vip3 => Platinum;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static Enumeration Vip4 => Black;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static Enumeration SVIP => Black;
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly CardTypeProvider Instance = new();
}
