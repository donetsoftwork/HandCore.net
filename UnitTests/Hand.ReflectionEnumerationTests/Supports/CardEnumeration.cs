using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Supports;

/// <summary>
/// Vip卡类型
/// </summary>
/// <param name="name"></param>
/// <param name="original"></param>
/// <param name="description"></param>
public class CardEnumeration(string name, long original, string description = "")
    : Enumeration(name, original, description)
{
    #region 配置
    #region Name
    /// <inheritdoc cref="Unknown" path="/summary"/>
    private const string UnknownName = "Unknown";
    /// <inheritdoc cref="Silver" path="/summary"/>
    private const string SilverName = "Silver";
    /// <inheritdoc cref="Gold" path="/summary"/>
    private const string GoldName = "Gold";
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private const string PlatinumName = "Platinum";
    /// <inheritdoc cref="Black" path="/summary"/>
    private const string BlackName = "Black";
    ///// <summary>
    ///// 标识
    ///// </summary>
    //public static string[] Names
    //    => [SilverName, GoldName, PlatinumName, BlackName];
    #endregion
    #region Original
    /// <inheritdoc cref="Unknown" path="/summary"/>
    private const long UnknownOriginal = 0;
    /// <inheritdoc cref="Silver" path="/summary"/>
    private const long SilverOriginal = 1;
    /// <inheritdoc cref="Gold" path="/summary"/>
    private const long GoldOriginal = 2;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private const long PlatinumOriginal = 3;
    /// <inheritdoc cref="Black" path="/summary"/>
    private const long BlackOriginal = 4;
    ///// <summary>
    ///// 原始值
    ///// </summary>
    //public static long[] Originals
    //    => [SilverOriginal, GoldOriginal, PlatinumOriginal, BlackOriginal];
    #endregion
    #region Enumeration
    /// <summary>
    /// 未知卡
    /// </summary>
    public static readonly CardEnumeration Unknown = new(UnknownName, UnknownOriginal, "未知卡");
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly CardEnumeration Silver = new(SilverName, SilverOriginal, "银卡");
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly CardEnumeration Gold = new(GoldName, GoldOriginal, "金卡");
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly CardEnumeration Platinum = new(PlatinumName, PlatinumOriginal, "铂金卡");
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly CardEnumeration Black = new(BlackName, BlackOriginal, "黑金卡");
    ///// <summary>
    ///// 所有枚举项
    ///// </summary>
    //public static CardEnumeration[] Items
    //    => [Silver, Gold, Platinum, Black];
    #endregion
    ///// <summary>
    ///// 枚举项数
    ///// </summary>
    //public static int Count
    //    => 4;
    #endregion
    /// <summary>
    /// 是否为未知枚举
    /// </summary>
    public bool IsUnknown
        => _original == UnknownOriginal;
    ///// <summary>
    ///// 获取枚举
    ///// </summary>
    ///// <param name="name"></param>
    ///// <param name="defaultValue"></param>
    ///// <returns></returns>
    //public static CardEnumeration Get(string name, CardEnumeration defaultValue)
    //    => name switch
    //{
    //    SilverName => Silver,
    //    GoldName => Gold,
    //    PlatinumName => Platinum,
    //    BlackName => Black,
    //    _ => defaultValue,
    //};
    ///// <summary>
    ///// 获取枚举
    ///// </summary>
    ///// <param name="name"></param>
    ///// <returns></returns>
    //public static CardEnumeration Get(string name)
    //    => name switch
    //    {
    //        SilverName => Silver,
    //        GoldName => Gold,
    //        PlatinumName => Platinum,
    //        BlackName => Black,
    //        _ => Unknown,
    //    };
    ///// <summary>
    ///// 获取枚举
    ///// </summary>
    ///// <param name="name"></param>
    ///// <param name="defaultValue"></param>
    ///// <returns></returns>
    //public static CardEnumeration Get(long original, CardEnumeration defaultValue)
    //    => original switch
    //{
    //    SilverOriginal => Silver,
    //    GoldOriginal => Gold,
    //    PlatinumOriginal => Platinum,
    //    BlackOriginal => Black,
    //    _ => defaultValue,
    //};
    ///// <summary>
    ///// 获取枚举
    ///// </summary>
    ///// <param name="name"></param>
    ///// <returns></returns>
    //public static CardEnumeration Get(long original)
    //    => original switch
    //    {
    //        SilverOriginal => Silver,
    //        GoldOriginal => Gold,
    //        PlatinumOriginal => Platinum,
    //        BlackOriginal => Black,
    //        _ => Unknown,
    //    };
}