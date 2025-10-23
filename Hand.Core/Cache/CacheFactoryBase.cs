#if NET9_0_OR_GREATER
using System.Threading;
#endif

namespace Hand.Cache;

/// <summary>
/// 缓存工厂基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class CacheFactoryBase<TKey, TValue>(ICacher<TKey, TValue> cacher)
    : ICacher<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public CacheFactoryBase()
        : this(new DictionaryCacher<TKey, TValue>())
    { 
    }
    /// <summary>
    /// 缓存字典
    /// </summary>
    private readonly ICacher<TKey, TValue> _cacher = cacher;
#if NET9_0_OR_GREATER
    private readonly Lock _cacherLock = new();
#endif
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual TValue Get(in TKey key)
    {
        if (_cacher.TryGetCache(key, out TValue value))
            return value;
#if NET9_0_OR_GREATER
        lock (_cacherLock)
#else
        lock (_cacher)
#endif
        {
            if (_cacher.TryGetCache(key, out value))
                return value;
            value = CreateNew(key);
            _cacher.Save(key, value);
        }
        return value;
    }
    /// <inheritdoc />
    public bool ContainsKey(in TKey key)
        => _cacher.ContainsKey(key);
    /// <inheritdoc />
    public bool TryGetCache(in TKey key, out TValue cached)
        => _cacher.TryGetCache(key, out cached);
    /// <inheritdoc />
    public void Save(in TKey key, TValue value)
        => _cacher.Save(key, value);
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TValue CreateNew(in TKey key);
}
