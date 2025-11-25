#if NET9_0_OR_GREATER
using System.Threading;
#endif

using Hand.Storage;

namespace Hand.Cache;

/// <summary>
/// 缓存工厂基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class CacheFactoryBase<TKey, TValue>(ICacher<TKey, TValue> cacher)
    : AutoCacheBase<TKey, TValue>(cacher)
    , IDataGet<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 缓存工厂基类
    /// </summary>
    public CacheFactoryBase()
        : this(new DictionaryCacher<TKey, TValue>())
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
    public virtual TValue Get(TKey key)
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
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TValue CreateNew(in TKey key);
}
