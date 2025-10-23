using System.Collections.Concurrent;

namespace Hand.Cache;

/// <summary>
/// 字典缓存
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="provider"></param>
public class DictionaryCacher<TKey, TValue>(IDictionary<TKey, TValue> provider)
    : ICacher<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 字典缓存
    /// </summary>
    public DictionaryCacher()
        : this(new ConcurrentDictionary<TKey, TValue>())
    {
    }
    #region 配置
    /// <summary>
    /// 存储字典
    /// </summary>
    private readonly IDictionary<TKey, TValue> _provider = provider;
    #endregion
    #region ISettings<TKey, TValue>
    /// <inheritdoc />
    public bool ContainsKey(in TKey key)
        => _provider.ContainsKey(key);
    /// <inheritdoc />
    public bool TryGetCache(in TKey key, out TValue cached)
        => _provider.TryGetValue(key, out cached);
    /// <inheritdoc />
    public void Save(in TKey key, TValue value)
        => _provider[key] = value;
    #endregion
}
