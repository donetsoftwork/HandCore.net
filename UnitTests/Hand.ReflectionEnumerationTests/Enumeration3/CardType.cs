using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enumeration3;

/// <summary>
/// Vip卡类型
/// </summary>
public abstract class CardType : Enumeration
{
    private CardType(string name, long original, string description = "")
        : base(name, original, description)
    {
    }
    /// <summary>
    /// 折扣
    /// </summary>
    public abstract decimal Discount { get; }

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

    #region 实现类
    /// <inheritdoc cref="Silver" path="/summary"/>
    private class SilverType()
        : CardType(nameof(Silver), 1, "银卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.9m;
    }
    /// <inheritdoc cref="Gold" path="/summary"/>
    private class GoldType()
        : CardType(nameof(Gold), 2, "金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.85m;
    }
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private class PlatinumType()
        : CardType(nameof(Platinum), 3, "铂金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.8m;
    }
    /// <inheritdoc cref="Black" path="/summary"/>
    private class BlackType()
        : CardType(nameof(Black), 4,  "黑金卡")
    {
        /// <inheritdoc />
        public override decimal Discount => 0.75m;
    }
    #endregion
}