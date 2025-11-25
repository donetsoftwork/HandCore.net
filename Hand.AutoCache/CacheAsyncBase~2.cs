using Hand.Storage;

namespace Hand.Cache;

/// <summary>
/// 缓存异步基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="cacher"></param>
public abstract class CacheAsyncBase<TKey, TValue>(ICacher<TKey, TValue> cacher)
    : AutoCacheBase<TKey, TValue>(cacher)
    , IAsyncGet<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 缓存异步基类
    /// </summary>
    public CacheAsyncBase()
        : this(new DictionaryCacher<TKey, TValue>())
    {
    }
    /// <summary>
    /// 信号量
    /// </summary>
    private readonly SemaphoreSlim _semaphore = new(1);
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<TValue> Get(TKey key, CancellationToken token = default)
    {
        if (_cacher.TryGetCache(key, out TValue value))
            return value;
        // 异步等待锁
        await _semaphore.WaitAsync();
        try
        {
            if (_cacher.TryGetCache(key, out value))
                return value;
            value = await CreateAsync(key, token);
            _cacher.Save(key, value);
            return value;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    protected abstract Task<TValue> CreateAsync(TKey key, CancellationToken token);
}
