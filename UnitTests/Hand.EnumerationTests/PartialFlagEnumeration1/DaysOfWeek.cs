using Hand.Enumerations;

namespace Hand.EnumerationTests.PartialFlagEnumeration1;

/// <summary>
/// 星期几枚举
/// </summary>
public sealed partial class DaysOfWeek
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
    private DaysOfWeek(string name, long original, string description = "")
         : base(name, original, description)
    {
    }
    #region Days
    /// <summary>
    /// 未知
    /// </summary>
    public static readonly DaysOfWeek Unknown = new(nameof(Unknown), 0, "未知");
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeek Sunday = new(nameof(Sunday), 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeek Monday = new(nameof(Monday), 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeek Tuesday = new(nameof(Tuesday), 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeek Wednesday = new(nameof(Wednesday), 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeek Thursday = new(nameof(Thursday), 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeek Friday = new(nameof(Friday), 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeek Saturday = new(nameof(Saturday), 1 << 6, "星期六");
    #endregion
}
