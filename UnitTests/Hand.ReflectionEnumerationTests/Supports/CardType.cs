using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Supports;

public abstract class CardType : Enumeration
{
    private CardType(string name, long original, string description)
        : base(name, original, description)
    {
    }
    #region 配置
    /// <summary>
    /// 折扣
    /// </summary>
    public abstract decimal Discount { get; }

    /// <inheritdoc cref="Silver" path="/"/>
    public const string SilverName = "Silver";
    /// <inheritdoc cref="Gold" path="/"/>
    public const string GoldName = "Gold";
    /// <inheritdoc cref="Platinum" path="/"/>
    public const string PlatinumName = "Platinum";
    /// <inheritdoc cref="Black" path="/"/>
    public const string BlackName = "Black";
    /// <inheritdoc cref="Silver" path="/"/>
    public const long SilverOriginal = 1;
    /// <inheritdoc cref="Gold" path="/"/>
    public const long GoldOriginal = 2;
    /// <inheritdoc cref="Platinum" path="/"/>
    public const long PlatinumOriginal = 3;
    /// <inheritdoc cref="Black" path="/"/>
    public const long BlackOriginal = 4;

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
    private static readonly CardType[] _items =
    [
        Silver,
        Gold,
        Platinum,
        Black,
    ];
    private static readonly string[] _names =
    [
        SilverName,
        GoldName,
        PlatinumName,
        BlackName,
    ];
    private static readonly long[] _originals =
    [
        SilverOriginal,
        GoldOriginal,
        PlatinumOriginal,
        BlackOriginal,
    ];
    /// <summary>
    /// 所有枚举项
    /// </summary>
    public static CardType[] Items
        => _items;
    /// <summary>
    /// 枚举项数
    /// </summary>
    public static int Count
        => 4;
    /// <summary>
    /// 标识
    /// </summary>
    public static IEnumerable<string> Names
        => _names;
    /// <summary>
    /// 原始值
    /// </summary>
    public static IEnumerable<long> Originals
        => _originals;
    #endregion

    #region Get
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static CardType Get(string name, CardType defaultValue)
        => name switch
        {
            SilverName => Silver,
            GoldName => Gold,
            PlatinumName => Platinum,
            BlackName => Black,
            nameof(Vip) => Silver,
            nameof(Vip2) => Gold,
            nameof(Vip3) => Platinum,
            nameof(Vip4) => Black,
            _ => defaultValue,
        };
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static CardType Get(string name)
        => name switch
        {
            SilverName => Silver,
            GoldName => Gold,
            PlatinumName => Platinum,
            BlackName => Black,
            nameof(Vip) => Silver,
            nameof(Vip2) => Gold,
            nameof(Vip3) => Platinum,
            nameof(Vip4) => Black,
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, "无效的枚举标识符"),
        };
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static CardType Get(long original, CardType defaultValue)
        => original switch
        {
            SilverOriginal => Silver,
            GoldOriginal => Gold,
            PlatinumOriginal => Platinum,
            BlackOriginal => Black,
            _ => defaultValue,
        };
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static CardType Get(long original)
        => original switch
        {
            SilverOriginal => Silver,
            GoldOriginal => Gold,
            PlatinumOriginal => Platinum,
            BlackOriginal => Black,
            _ => throw new ArgumentOutOfRangeException(nameof(original), original, "无效的枚举值"),
        };
    #endregion
    /// <inheritdoc cref="Silver" path="/summary"/>
    private class SilverType()
        : CardType(SilverName, SilverOriginal, "银卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.9m;
    }
    /// <inheritdoc cref="Gold" path="/summary"/>
    private class GoldType()
        : CardType(GoldName, GoldOriginal, "金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.85m;
    }
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private class PlatinumType()
        : CardType(PlatinumName, PlatinumOriginal, "铂金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.8m;
    }
    /// <inheritdoc cref="Black" path="/summary"/>
    private class BlackType()
        : CardType(BlackName, BlackOriginal, "黑金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.75m;
    }
}