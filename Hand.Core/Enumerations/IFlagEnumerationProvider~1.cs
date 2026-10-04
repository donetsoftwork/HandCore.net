using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
public interface IFlagEnumerationProvider<TEnumeration>
    : IEnumerationProvider<TEnumeration>
    where TEnumeration : IFlagEnumeration
{
    /// <summary>
    /// 位标记
    /// </summary>
    IEnumerable<TEnumeration> Flags { get; }
    /// <summary>
    /// 空枚举
    /// </summary>
    TEnumeration Empty { get; }
    #region Get
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    TEnumeration Get(string name);
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    TEnumeration Get(long original);
    #endregion
    #region Parse
    /// <summary>
    /// 解析枚举名
    /// </summary>
    /// <param name="name">枚举名</param>
    /// <returns></returns>
    TEnumeration Parse(string name);
    /// <summary>
    /// 解析枚举值
    /// </summary>
    /// <param name="original">枚举值</param>
    /// <returns></returns>
    TEnumeration Parse(long original);
    #endregion
    /// <summary>
    /// 按位或操作
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    TEnumeration And(TEnumeration left, TEnumeration right, string description = "");
    /// <summary>
    /// 或运算
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    TEnumeration Or(TEnumeration left, TEnumeration right, string description = "");
}
