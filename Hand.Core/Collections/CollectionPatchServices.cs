namespace Hand.Collections;

/// <summary>
/// 集合补丁扩展方法
/// </summary>
public static partial class HandCoreCollectionServices
{
#if NETSTANDARD2_0 || NETFRAMEWORK
    /// <summary>
    /// 获取值或默认值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        => dictionary.GetValueOrDefault(key, default!);
    /// <summary>
    /// 获取值或默认值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        => dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    /// <summary>
    /// 尝试添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            return false;
        dictionary.Add(key, value);
        return true;
    }
#endif
}
