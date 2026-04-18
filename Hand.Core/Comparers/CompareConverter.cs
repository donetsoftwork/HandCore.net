#if NET7_0_OR_GREATER
using System.Collections.Concurrent;
#endif

namespace Hand.Comparers;

/// <summary>
/// 比较器服务
/// </summary>
public static class CompareConverter
{
    /// <summary>
    /// 转化为StringComparer
    /// </summary>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static StringComparer ToComparer(StringComparison comparison)
    {
        return comparison switch
        {
            StringComparison.Ordinal => StringComparer.Ordinal,
            StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
            StringComparison.CurrentCulture => StringComparer.CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
            _ => StringComparer.Ordinal,
        };
    }
    /// <summary>
    /// 转化为StringComparison
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static StringComparison ToComparison(StringComparer comparer)
    {
        if (comparer == StringComparer.Ordinal)
            return StringComparison.Ordinal;
        else if (comparer == StringComparer.OrdinalIgnoreCase)
            return StringComparison.OrdinalIgnoreCase;
        else if (comparer == StringComparer.CurrentCulture)
            return StringComparison.CurrentCulture;
        else if (comparer == StringComparer.CurrentCultureIgnoreCase)
            return StringComparison.CurrentCultureIgnoreCase;
        else
            return StringComparison.Ordinal;
    }
    /// <summary>
    /// 获取字典比较器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static IEqualityComparer<TKey> GetComparer<TKey, TValue>(IDictionary<TKey, TValue> owner)
    {
        if (owner is null)
            return EqualityComparer<TKey>.Default;
        if (owner is Dictionary<TKey, TValue> dictionary)
            return dictionary.Comparer;
#if NET7_0_OR_GREATER
        if (owner is ConcurrentDictionary<TKey, TValue> concurrentDictionary)
            return concurrentDictionary.Comparer;
#endif
        return EqualityComparer<TKey>.Default;
    }
    /// <summary>
    /// 获取集合比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static IEqualityComparer<T> GetComparer<T>(ISet<T> owner)
    {
        if (owner is null)
            return EqualityComparer<T>.Default;
        if (owner is HashSet<T> hashSet)
            return hashSet.Comparer;
        return EqualityComparer<T>.Default;
    }
}
