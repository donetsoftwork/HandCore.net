using Hand.Enumerations;

namespace Hand.EnumerationTests.Enumeration1;

/// <summary>
/// Vip枚举
/// </summary>
public class CardType : Enumeration
{
    private CardType(string name, long original, string description)
        : base(name, original, description)
    {
    }
    #region 配置
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
    #endregion   
    #region Provider
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    public static readonly IEnumerationProvider<CardType> Provider = new CardTypeProvider();
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    private class CardTypeProvider : EnumerationProvider<CardType>
    {
        internal CardTypeProvider()
            : base(
            [Silver, Gold, Platinum, Black]
            , new Dictionary<string, CardType>(StringComparer.Ordinal)
            {
                { nameof(Silver), Silver},
                { nameof(Gold), Gold},
                { nameof(Platinum), Platinum},
                { nameof(Black), Black},
                { nameof(Vip) , Silver},
                { nameof(Vip2) , Gold},
                { nameof(Vip3) , Platinum},
                { nameof(Vip4) , Black},
            }
            , new Dictionary<long, CardType>()
            {
                { Silver.Original, Silver},
                { Gold.Original, Gold},
                { Platinum.Original, Platinum},
                { Black.Original, Black}
            }
        )
        {
        }
    }
    #endregion
}