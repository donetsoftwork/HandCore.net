using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.FlagEnumeration2;

public class DaysOfWeek
    : FlagEnumeration
{
    /// <summary>
    /// 这个构造函数是关键,没有这个构造函数会报错
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    public DaysOfWeek(ISet<string> flags, long original, string description = "")
        : base(flags, original, description)
    {
    }
    public DaysOfWeek(IEqualityComparer<string> comparer, string name, long original, string description = "")
         : base(comparer, name, original, description)
    {
    }
    private static readonly IEqualityComparer<string> _comparer = StringComparer.Ordinal;
    #region Days
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeek Sunday = new(_comparer, "Sunday", 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeek Monday = new(_comparer, "Monday", 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeek Tuesday = new(_comparer, "Tuesday", 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeek Wednesday = new(_comparer, "Wednesday", 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeek Thursday = new(_comparer, "Thursday", 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeek Friday = new(_comparer, "Friday", 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeek Saturday = new(_comparer, "Saturday", 1 << 6, "星期六");
    #endregion


}
