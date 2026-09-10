using Hand.Enumerations;

namespace Hand.EnumerationTests.Supports;

/// <summary>
/// 星期几枚举
/// </summary>
public class DaysOfWeekEnumeration
    : FlagEnumeration
{
    public DaysOfWeekEnumeration(ISet<string> flags, long original, string description = "")
        : base(flags, original, description)
    {
    }
    private DaysOfWeekEnumeration(IEqualityComparer<string> comparer, string name, long original, string description = "")
         : base(comparer, name, original, description)
    {
    }
    #region 配置
    #region Name
    private const string UnknownName = "Unknown";
    private const string SundayName = "Sunday";
    private const string MondayName = "Monday";
    private const string TuesdayName = "Tuesday";
    private const string WednesdayName = "Wednesday";
    private const string ThursdayName = "Thursday";
    private const string FridayName = "Friday";
    private const string SaturdayName = "Saturday";
    #endregion
    #region Original
    private const long UnknownOriginal = 0;
    private const long SundayOriginal = 1;
    private const long MondayOriginal = 1 << 1;
    private const long TuesdayOriginal = 1 << 2;
    private const long WednesdayOriginal = 1 << 3;
    private const long ThursdayOriginal = 1 << 4;
    private const long FridayOriginal = 1 << 5;
    private const long SaturdayOriginal = 1 << 6;
    #endregion
    private static readonly IEqualityComparer<string> _comparer = StringComparer.Ordinal;
    #region Enumeration
    /// <summary>
    /// 未知
    /// </summary>
    public static readonly DaysOfWeekEnumeration Unknown = new(_comparer, UnknownName, UnknownOriginal, "未知");
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeekEnumeration Sunday = new(_comparer, SundayName, SundayOriginal, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeekEnumeration Monday = new(_comparer, MondayName, MondayOriginal, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeekEnumeration Tuesday = new(_comparer, TuesdayName, TuesdayOriginal, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeekEnumeration Wednesday = new(_comparer, WednesdayName, WednesdayOriginal, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeekEnumeration Thursday = new(_comparer, ThursdayName, ThursdayOriginal, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeekEnumeration Friday = new(_comparer, FridayName, FridayOriginal, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeekEnumeration Saturday = new(_comparer, SaturdayName, SaturdayOriginal, "星期六");
    #endregion
    #endregion 
    ///// <summary>
    ///// 按位或操作
    ///// </summary>
    ///// <param name="flag"></param>
    ///// <param name="description"></param>
    ///// <returns></returns>
    //public DaysOfWeekEnumeration And(DaysOfWeekEnumeration flag, string description = "")
    //{
    //    var flagOriginal = flag._original;
    //    if (_original == flagOriginal)
    //        return this;
    //    if (_original == UnknownOriginal || flagOriginal == UnknownOriginal)
    //        return Unknown;

    //    var combinedOriginal = _original & flag.Original;
    //    if (combinedOriginal == UnknownOriginal)
    //        return Unknown;
    //    var days = Provider.Instance.Get(combinedOriginal);
    //    if (days is null)
    //    {
    //        var combinedFlags = new HashSet<string>(_flags.Except(flag.Flags, _comparer), _comparer);
    //        return new(combinedFlags, combinedOriginal, description);
    //    }
    //    return days;
    //}
    ///// <inheritdoc />
    //public override FlagEnumeration And<TEnumeration>(TEnumeration flag, string description = "")
    //    => flag is DaysOfWeekEnumeration days ? And(days, description) : Unknown;
    ///// <summary>
    ///// 按位或操作
    ///// </summary>
    ///// <param name="flag"></param>
    ///// <param name="description"></param>
    ///// <returns></returns>
    //public DaysOfWeekEnumeration Or(DaysOfWeekEnumeration flag, string description = "")
    //{
    //    var flagOriginal = flag._original;
    //    if (_original == flagOriginal || flagOriginal == UnknownOriginal)
    //        return this;
    //    if (_original == UnknownOriginal)
    //        return flag;

    //    var combinedFlags = new HashSet<string>(_flags, _comparer);
    //    foreach (var flagName in flag.Flags)
    //    {
    //        combinedFlags.Add(flagName);
    //    }
    //    return new(combinedFlags, _original | flag.Original, description);
    //}
    ///// <inheritdoc />
    //public override FlagEnumeration Or<TEnumeration>(TEnumeration flag, string description = "")
    //    => flag is DaysOfWeekEnumeration days ? Or(days, description) : this;
    /// <summary>
    /// 是否为未知枚举
    /// </summary>
    public bool IsUnknown
        => _original == UnknownOriginal;

    /// <summary>
    /// 星期几枚举提供者
    /// </summary>
    public sealed class Provider : FlagEnumerationProvider<DaysOfWeekEnumeration>
    {
        private Provider()
            : base(
            [Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday]
            , new Dictionary<string, DaysOfWeekEnumeration>(_comparer)
            {
                { UnknownName, Unknown },
                { SundayName, Sunday},
                { MondayName, Monday},
                { TuesdayName, Tuesday},
                { WednesdayName, Wednesday},
                { ThursdayName, Thursday},
                { FridayName, Friday},
                { SaturdayName, Saturday}
            }
            , new Dictionary<long, DaysOfWeekEnumeration>()
            {
                { UnknownOriginal, Unknown },
                { SundayOriginal, Sunday},
                { MondayOriginal, Monday},
                { TuesdayOriginal, Tuesday},
                { WednesdayOriginal, Wednesday},
                { ThursdayOriginal, Thursday},
                { FridayOriginal, Friday},
                { SaturdayOriginal, Saturday}
            }
        )
        {
        }
        /// <inheritdoc />
        public override DaysOfWeekEnumeration Empty => Unknown;
        /// <inheritdoc />
        protected override DaysOfWeekEnumeration CreateFlag(ISet<string> flags, long original, string description = "")
            => new(flags, original, description);
        /// <inheritdoc />
        public override DaysOfWeekEnumeration Get(string name, DaysOfWeekEnumeration defaultValue)
        {
            var value = base.Get(name, defaultValue);
            if (value.Original == UnknownOriginal)
                return defaultValue;
            return value;
        }
        /// <inheritdoc />
        public override DaysOfWeekEnumeration Get(long original, DaysOfWeekEnumeration defaultValue)
        {
            var value = base.Get(original, defaultValue);
            if (value.Original == UnknownOriginal)
                return defaultValue;
            return value;
        }
        /// <summary>
        /// 静态实例
        /// </summary>
        public static readonly Provider Instance = new();
    }
}
