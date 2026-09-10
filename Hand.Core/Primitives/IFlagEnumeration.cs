namespace Hand.Primitives;

/// <summary>
/// 位标记枚举接口
/// </summary>
public interface IFlagEnumeration : IEnumeration
{
    /// <summary>
    /// 枚举名
    /// </summary>
    IEnumerable<string> Names { get; }
    /// <summary>
    /// 判断是否包含指定标记
    /// </summary>
    /// <param name="flag">标记</param>
    /// <returns></returns>
    bool HasFlag(long flag);
    /// <summary>
    /// 判断是否包含指定枚举名
    /// </summary>
    /// <param name="flag">枚举名</param>
    /// <returns></returns>
    bool HasFlag(string flag);
}
