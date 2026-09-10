using Hand.Enumerations;

namespace Hand.EnumerationTests.FlagEnumeration2;

/// <summary>
/// 星期几枚举
/// </summary>
public sealed class DaysOfWeek
    : FlagEnumeration
{
    /// <summary>
    /// 这个构造函数是关键,没有这个构造函数会报错
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    private DaysOfWeek(ISet<string> flags, long original, string description = "")
        : base(flags, original, description)
    {
    }
    private DaysOfWeek(IEqualityComparer<string> comparer, string name, long original, string description = "")
         : base(comparer, name, original, description)
    {
    }
    private static readonly IEqualityComparer<string> _comparer = StringComparer.Ordinal;
    #region Days
    /// <summary>
    /// 未知
    /// </summary>
    public static readonly DaysOfWeek Unknown = new(_comparer, nameof(Unknown), 0, "未知");
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeek Sunday = new(_comparer, nameof(Sunday), 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeek Monday = new(_comparer, nameof(Monday), 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeek Tuesday = new(_comparer, nameof(Tuesday), 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeek Wednesday = new(_comparer, nameof(Wednesday), 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeek Thursday = new(_comparer, nameof(Thursday), 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeek Friday = new(_comparer, nameof(Friday), 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeek Saturday = new(_comparer, nameof(Saturday), 1 << 6, "星期六");
    #endregion
    /// <summary>
    /// 或运算
    /// </summary>
    /// <param name="others"></param>
    /// <returns></returns>
    public DaysOfWeek Or(params DaysOfWeek[] others)
        => Provider.Or(this, others);
    /// <summary>
    /// 与运算
    /// </summary>
    /// <param name="others"></param>
    /// <returns></returns>
    public DaysOfWeek And(params DaysOfWeek[] others)
        => Provider.And(this, others);

    /// <summary>
    /// 静态实例
    /// </summary>
    public static readonly IFlagEnumerationProvider<DaysOfWeek> Provider = new DaysOfWeekProvider();
    /// <summary>
    /// 星期几枚举提供者
    /// </summary>
    private sealed class DaysOfWeekProvider : FlagEnumerationProvider<DaysOfWeek>
    {
        internal DaysOfWeekProvider()
            : base(
            [Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday]
            , new Dictionary<string, DaysOfWeek>(_comparer)
            {
                { nameof(Unknown), Unknown },
                { nameof(Sunday), Sunday},
                { nameof(Monday), Monday},
                { nameof(Tuesday), Tuesday},
                { nameof(Wednesday), Wednesday},
                { nameof(Thursday),Thursday},
                { nameof(Friday), Friday},
                { nameof(Saturday), Saturday}
            }
            , new Dictionary<long, DaysOfWeek>()
            {
                { Unknown.Original, Unknown },
                { Sunday.Original, Sunday},
                { Monday.Original, Monday},
                { Tuesday.Original, Tuesday},
                { Wednesday.Original, Wednesday},
                { Thursday.Original, Thursday},
                { Friday.Original, Friday},
                { Saturday.Original, Saturday}
            }
        )
        {
        }
        /// <inheritdoc />
        public override DaysOfWeek Empty => Unknown;
        /// <inheritdoc />
        protected override DaysOfWeek CreateFlag(ISet<string> flags, long original, string description = "")
            => new(flags, original, description);
    }
}
