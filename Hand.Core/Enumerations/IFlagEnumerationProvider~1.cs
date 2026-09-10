using Hand.Convert;
using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
public interface IFlagEnumerationProvider<TEnumeration>
    : IEnumerationProvider<TEnumeration>, IParser<string, TEnumeration?>, IParser<long, TEnumeration?>
    where TEnumeration : IFlagEnumeration
{
    /// <summary>
    /// 位标记
    /// </summary>
    TEnumeration[] Flags { get; }
    /// <summary>
    /// 空枚举
    /// </summary>
    TEnumeration Empty { get; }
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
