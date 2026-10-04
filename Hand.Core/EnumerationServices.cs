using Hand.Enumerations;
using Hand.Primitives;
using System.Runtime.CompilerServices;

namespace Hand;

/// <summary>
/// 枚举扩展方法
/// </summary>
public static partial class HandCoreServices
{
    #region Get
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static TEnumeration? Get<TEnumeration>(this IEnumerationProvider<TEnumeration> provider, string name)
        where TEnumeration : IEnumeration
    {
        provider.TryGet(name, out var value);
        return value;
    }
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public static TEnumeration? Get<TEnumeration>(this IEnumerationProvider<TEnumeration> provider, long original)
        where TEnumeration : IEnumeration
    {
        provider.TryGet(original, out var value);
        return value;
    }
    #endregion
    /// <summary>
    /// 判断是否包含指定位域
    /// </summary>
    /// <param name="enumeration"></param>
    /// <param name="flag">位域</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag<TEnumeration>(this TEnumeration enumeration, TEnumeration flag)
        where TEnumeration : IFlagEnumeration
        => enumeration.HasFlag(flag.Original);
    /// <summary>
    /// 或运算
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="provider"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static TEnumeration Or<TEnumeration>(this IFlagEnumerationProvider<TEnumeration> provider, TEnumeration left, params TEnumeration[] right)
        where TEnumeration : IFlagEnumeration
    {
        foreach (var item in right)
            left = provider.Or(left, item);
        return left;
    }
    /// <summary>
    /// 与运算
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="provider"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static TEnumeration And<TEnumeration>(this IFlagEnumerationProvider<TEnumeration> provider, TEnumeration left, params TEnumeration[] right)
        where TEnumeration : IFlagEnumeration
    {
        foreach (var item in right)
            left = provider.And(left, item);
        return left;
    }
}
