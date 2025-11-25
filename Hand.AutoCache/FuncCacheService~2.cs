namespace Hand.Cache;

/// <summary>
/// 委托缓存服务
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="cacher"></param>
/// <param name="func"></param>
public class FuncCacheService<TKey, TValue>(ICacher<TKey, TValue> cacher, Func<TKey, TValue> func)
    : CacheFactoryBase<TKey, TValue>(cacher)
    where TKey : notnull
{
    /// <summary>
    /// 委托缓存服务
    /// </summary>
    /// <param name="func"></param>
    public FuncCacheService(Func<TKey, TValue> func)
        : this(new DictionaryCacher<TKey, TValue>(), func)
    {
    }
    #region 配置
    private readonly Func<TKey, TValue> _func = func;
    /// <summary>
    /// 委托
    /// </summary>
    public Func<TKey, TValue> Func 
        => _func;
    #endregion
    #region CacheFactoryBase<TKey, TValue>
    /// <inheritdoc />
    protected override TValue CreateNew(in TKey key)
        => _func(key);
    #endregion
}
