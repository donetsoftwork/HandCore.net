namespace Hand.Cache;

/// <summary>
/// 自动缓存基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="cacher"></param>
public abstract class AutoCacheBase<TKey, TValue>(ICacher<TKey, TValue> cacher)
    : ICacher<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 缓存字典
    /// </summary>
    protected readonly ICacher<TKey, TValue> _cacher = cacher;
    /// <inheritdoc />
    public bool ContainsKey(in TKey key)
        => _cacher.ContainsKey(key);
    /// <inheritdoc />
    public bool TryGetCache(in TKey key, out TValue cached)
        => _cacher.TryGetCache(key, out cached);
    /// <inheritdoc />
    public void Save(in TKey key, TValue value)
        => _cacher.Save(key, value);
}