namespace Hand.Cache;

/// <summary>
/// 自动缓存基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="cacher"></param>
public abstract class AutoCacheBase<TKey>(IGenericCacher<TKey> cacher)
    : IGenericCacher<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 缓存字典
    /// </summary>
    protected readonly IGenericCacher<TKey> _cacher = cacher;
    /// <inheritdoc />
    public abstract TValue Get<TValue>(TKey spec);
    //{
    //    _cacher.TryGetCache<TValue>(spec, out var value);
    //    return value;
    //}
    /// <inheritdoc />
    public bool Contains(in TKey key)
        => _cacher.Contains(key);
    /// <inheritdoc />
    public bool TryGetCache<TValue>(in TKey key, out TValue? cached)
        => _cacher.TryGetCache(key, out cached);
    /// <inheritdoc />
    public void Save<TValue>(in TKey key, TValue? value)
        => _cacher.Save(key, value);
}