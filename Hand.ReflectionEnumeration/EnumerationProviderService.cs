using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 枚举提供者服务
/// </summary>
public static partial class ReflectionEnumeration
{
    /// <summary>
    /// 转化为枚举字典
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="list"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static EnumerationProvider<TEnumeration> ToProvider<TEnumeration>(this IEnumerable<TEnumeration> list, IEqualityComparer<string>? comparer = null)
        where TEnumeration : IEnumeration
    {
        var (names, originals) = ToDictionary(list, comparer);
        return new([.. originals.Values], names, originals);
    }
    /// <summary>
    /// 转化为枚举字典
    /// </summary>
    /// <param name="list"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static FlagEnumerationProvider ToFlagProvider(this IEnumerable<FlagEnumeration> list, IEqualityComparer<string>? comparer = null)
    {
        var (names, originals) = ToDictionary(list, comparer);
        return new([.. originals.Values], names, originals);
    }
    /// <summary>
    /// 转化为枚举字典
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="list"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static FlagReflectionProvider<TEnumeration> ToReflectionProvider<TEnumeration>(this IEnumerable<TEnumeration> list, IEqualityComparer<string>? comparer = null)
        where TEnumeration : IFlagEnumeration
    {
        var (names, originals) = ToDictionary(list, comparer);
        return new([.. originals.Values], names, originals);
    }
    /// <summary>
    /// 转化为字典
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="list"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    private static (IReadOnlyDictionary<string, TEnumeration>, IReadOnlyDictionary<long, TEnumeration>) ToDictionary<TEnumeration>(IEnumerable<TEnumeration> list, IEqualityComparer<string>? comparer = null)
         where TEnumeration : IEnumeration
    {
        var names = new Dictionary<string, TEnumeration>(comparer ?? StringComparer.Ordinal);
        var originals = new Dictionary<long, TEnumeration>();
        foreach (var item in list)
        {
            var original = item.Original;
            if (originals.TryGetValue(original, out var enumeration))
            {
                // 处理枚举别名
                names[item.Name] = enumeration;
                continue;
            }
            originals.Add(original, item);
            names[item.Name] = item;
        }
        return (names, originals);
    }
}
