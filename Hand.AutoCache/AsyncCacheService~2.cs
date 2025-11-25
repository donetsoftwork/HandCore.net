namespace Hand.Cache;

/// <summary>
/// 异步委托缓存服务
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="cacher"></param>
/// <param name="func"></param>
public class AsyncCacheService<TKey, TValue>(ICacher<TKey, TValue> cacher, Func<TKey, CancellationToken, Task<TValue>> func)
    : CacheAsyncBase<TKey, TValue>(cacher)
    where TKey : notnull
{
    /// <summary>
    /// 委托缓存服务
    /// </summary>
    /// <param name="func"></param>
    public AsyncCacheService(Func<TKey, CancellationToken, Task<TValue>> func)
        : this(new DictionaryCacher<TKey, TValue>(), func)
    {
    }
    #region 配置
    private readonly Func<TKey, CancellationToken, Task<TValue>> _func = func;
    /// <summary>
    /// 委托
    /// </summary>
    public Func<TKey, CancellationToken, Task<TValue>> Func
        => _func;
    #endregion
    #region CacheAsyncBase<TKey, TValue>
    /// <inheritdoc />
    protected override Task<TValue> CreateAsync(TKey key, CancellationToken token)
     => _func(key, token);
    #endregion
}
