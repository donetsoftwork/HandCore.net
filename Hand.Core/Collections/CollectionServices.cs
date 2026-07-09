using Hand.Comparers;
using System.Runtime.CompilerServices;

namespace Hand.Collections;

/// <summary>
/// 集合扩展方法
/// </summary>
public static partial class HandCoreCollectionServices
{
    /// <summary>
    /// 排除
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Except<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, ICollection<TKey> keys)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(CompareConverter.GetComparer(dictionary));
        foreach (var pair in dictionary)
        {
            var key = pair.Key;
            if (keys.Contains(key))
                continue;
            result.Add(key, pair.Value);
        }

        return result;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="group"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> group, TKey key, TValue value)
    {
        if (group.TryGetValue(key, out var list))
        {
            list.Add(value);
        }
        else
        {
            group[key] = [value];
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="group"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="valueComparer"></param>
    public static void Add<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> group, TKey key, TValue value, IEqualityComparer<TValue> valueComparer)
    {
        if (group.TryGetValue(key, out var set))
        {
            set.Add(value);
        }
        else
        {
            group[key] = new HashSet<TValue>(valueComparer) { value };
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="group"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> group, TKey key, TValue value)
        => Add(group, key, value, EqualityComparer<TValue>.Default);

//    public static ISet<TValue> ToSet<TValue>(this IEnumerable<TValue> values, IEqualityComparer<TValue> comparer)
//    {
//#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
//        return values.ToHashSet(comparer);
//#else
//        return new HashSet<TValue>(values, comparer);
//#endif
//    }
}
