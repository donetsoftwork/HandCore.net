namespace Hand.ReflectionEnumerationTests.FlagEnum1;

[Flags]
public enum DaysOfWeek
{
    /// <summary>
    /// 星期日
    /// </summary>
    Sunday = 1,
    /// <summary>
    /// 星期一
    /// </summary>
    Monday = 1 << 1,
    /// <summary>
    /// 星期二
    /// </summary>
    Tuesday = 1 << 2,
    /// <summary>
    /// 星期三
    /// </summary>
    Wednesday = 1 << 3,
    /// <summary>
    /// 星期四
    /// </summary>
    Thursday = 1 << 4,
    /// <summary>
    /// 星期五
    /// </summary>
    Friday = 1 << 5,
    /// <summary>
    /// 星期六
    /// </summary>
    Saturday = 1 << 6,
}
