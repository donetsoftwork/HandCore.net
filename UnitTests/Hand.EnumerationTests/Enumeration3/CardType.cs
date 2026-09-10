using Hand.Enumerations;

namespace Hand.EnumerationTests.Enumeration3;

/// <summary>
/// Vip枚举
/// </summary>
public abstract class CardType : Enumeration
{
    private CardType(string name, long original, string description)
        : base(name, original, description)
    {
    }
    #region 配置
    /// <inheritdoc cref="Unknown" path="/summary"/>
    public const long UnknownOriginal = 0;
    /// <inheritdoc cref="Silver" path="/summary"/>
    public const long SilverOriginal = 1;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public const long GoldOriginal = 2;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public const long PlatinumOriginal = 3;
    /// <inheritdoc cref="Black" path="/summary"/>
    public const long BlackOriginal = 4;
    /// <summary>
    /// 打折
    /// </summary>
    /// <param name="price"></param>
    /// <param name="discount"></param>
    /// <returns></returns>
    public abstract bool CheckDiscount(decimal price, ref decimal discount);
    /// <summary>
    /// 未知卡
    /// </summary>
    public static readonly CardType Unknown = new UnknownType();
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly CardType Silver = new SilverType();
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly CardType Gold = new GoldType();
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly CardType Platinum = new PlatinumType();
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly CardType Black = new BlackType();

    /// <inheritdoc cref="Silver" path="/summary"/>
    public static CardType Vip => Silver;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public static CardType Vip2 => Gold;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public static CardType Vip3 => Platinum;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static CardType Vip4 => Black;
    #endregion
    #region 实现类
    /// <inheritdoc cref="Unknown" path="/summary"/>
    private class UnknownType()
        : CardType(nameof(Unknown), UnknownOriginal, "未知")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
            => false;
    }
    /// <inheritdoc cref="Silver" path="/summary"/>
    private class SilverType()
        : CardType(nameof(Silver), 1, "银卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 银卡9折(不与其他折扣同享)
            if (discount > 0M)
                return false;
            discount = price * 0.1M;
            return true;
        }
    }
    /// <inheritdoc cref="Gold" path="/summary"/>
    private class GoldType()
        : CardType(nameof(Gold), 2, "金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 金卡最高85折
            var discount2 = price * 0.15M;
            if (discount2 > discount)
            {
                discount = discount2;
                return true;
            }
            return false;
        }
    }
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private class PlatinumType()
        : CardType(nameof(Platinum), 3, "铂金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 铂金卡最高8折
            var discount2 = price * 0.2M;
            if (discount2 > discount)
            {
                discount = discount2;
                return true;
            }
            return false;
        }
    }
    /// <inheritdoc cref="Black" path="/summary"/>
    private class BlackType()
        : CardType(nameof(Black), 4, "黑金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 黑金卡75折上折
            var price2 = price - discount;
            discount += price2 * 0.25M;
            return true;
        }
    }
    #endregion
    #region Provider
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    public static readonly IEnumerationProvider<CardType> Provider = new CardTypeProvider();
    /// <summary>
    /// Vip枚举提供者实现类
    /// </summary>
    class CardTypeProvider : EnumerationProvider<CardType>
    {
        internal CardTypeProvider()
            : base(
            [Silver, Gold, Platinum, Black]
            , new Dictionary<string, CardType>(StringComparer.Ordinal)
            {
            { nameof(Unknown), Unknown },
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
            { UnknownOriginal, Unknown },
            { SilverOriginal, Silver},
            { GoldOriginal, Gold},
            { PlatinumOriginal, Platinum},
            { BlackOriginal, Black}
            }
        )
        {
        }
        /// <inheritdoc />
        public override CardType Get(string name, CardType defaultValue)
        {
            var value = base.Get(name, defaultValue);
            if (value.Original == UnknownOriginal)
                return defaultValue;
            return value;
        }
        /// <inheritdoc />
        public override CardType Get(long original, CardType defaultValue)
        {
            var value = base.Get(original, defaultValue);
            if (value.Original == UnknownOriginal)
                return defaultValue;
            return value;
        }
    }
    #endregion
}