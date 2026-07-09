#if NET9_0_OR_GREATER
using System.Threading;
#endif

namespace Hand.Cache;

/// <summary>
/// 缓存工厂基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class CacheFactoryBase<TKey>(IGenericCacher<TKey> cacher)
    : AutoCacheBase<TKey>(cacher)    
    where TKey : notnull
{
    /// <summary>
    /// 缓存工厂基类
    /// </summary>
    public CacheFactoryBase()
        : this(new DictionaryCacher<TKey>())
    {
    }
#if NET9_0_OR_GREATER
    private readonly Lock _cacherLock = new();
#endif
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override TValue Get<TValue>(TKey key)
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
            value = CreateNew<TValue>(key);
            _cacher.Save(key, value);
        }
        return value;
    }
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TValue CreateNew<TValue>(in TKey key);
}
