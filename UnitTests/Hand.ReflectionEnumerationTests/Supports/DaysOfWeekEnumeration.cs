using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Supports;

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
    private static readonly IEqualityComparer<string> _comparer = StringComparer.Ordinal;
    #region Enumeration
    /// <summary>
    /// 未知
    /// </summary>
    public static readonly DaysOfWeekEnumeration Unknown = new(_comparer, "Unknown", 0, "未知");
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeekEnumeration Sunday = new(_comparer, "Sunday", 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeekEnumeration Monday = new(_comparer, "Monday", 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeekEnumeration Tuesday = new(_comparer, "Tuesday", 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeekEnumeration Wednesday = new(_comparer, "Wednesday", 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeekEnumeration Thursday = new(_comparer, "Thursday", 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeekEnumeration Friday = new(_comparer, "Friday", 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeekEnumeration Saturday = new(_comparer, "Saturday", 1 << 6, "星期六");
    #endregion   
}
