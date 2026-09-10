using System.ComponentModel;

namespace Hand.ReflectionEnumerationTests.FlagEnum2;

[Flags]
public enum DaysOfWeek
{
    /// <summary>
    /// 星期日
    /// </summary>
    [Description("周日")]
    Sunday = 1,
    /// <summary>
    /// 星期一
    /// </summary>
    [Description("周一")]
    Monday = 1 << 1,
    /// <summary>
    /// 星期二
    /// </summary>
    [Description("周二")]
    Tuesday = 1 << 2,
    /// <summary>
    /// 星期三
    /// </summary>
    [Description("周三")]
    Wednesday = 1 << 3,
    /// <summary>
    /// 星期四
    /// </summary>
    [Description("周四")]
    Thursday = 1 << 4,
    /// <summary>
    /// 星期五
    /// </summary>
    [Description("周五")]
    Friday = 1 << 5,
    /// <summary>
    /// 星期六
    /// </summary>
    [Description("周六")]
    Saturday = 1 << 6,
}
