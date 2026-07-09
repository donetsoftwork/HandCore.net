namespace Hand.Collections;

/// <summary>
/// 分组集合
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IGroupCollection<TKey, TValue>
{
    /// <summary>
    /// 分组键
    /// </summary>
    IEnumerable<TKey> Keys { get; }
    /// <summary>
    /// 分组是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool ContainsKey(TKey key);
    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEnumerable<TValue> GetValues(TKey key);
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Add(TKey key, TValue value);
}
