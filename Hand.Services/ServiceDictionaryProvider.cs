using System.Collections;

namespace Hand.Services;

/// <summary>
/// 字典服务存储(简易服务容器)
/// </summary>
/// <param name="provider"></param>
public class ServiceDictionaryProvider(IDictionary provider)
{
    /// <summary>
    /// 字典服务存储(简易服务容器)
    /// </summary>
    public ServiceDictionaryProvider()
        : this(new Hashtable())
    {
    }
    #region 配置
    /// <summary>
    /// 字典存储
    /// </summary>
    protected readonly IDictionary _provider = provider;
#if NET9_0_OR_GREATER
    private readonly Lock _providerLock = new();
#endif
    #endregion
    /// <summary>
    /// 添加服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="key"></param>
    /// <param name="service"></param>
    public void AddService<TService>(object key, TService service)
    {
        CheckServiceCollection<TService>(key)
            .Add(service);
    }
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEnumerable<TService> GetServices<TService>(object key)
    {
        if (_provider[key] is IEnumerable<TService> services)
            return services;
        return [];
    }
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TService GetService<TService>(object key)
    {
        var collection = _provider[key];
        if(collection is not null && collection is IEnumerable<TService> service)
            return service.FirstOrDefault();
        return default;
    }
    /// <summary>
    /// 获取服务集合
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected ICollection<TService> CheckServiceCollection<TService>(object key)
    {
        if (_provider[key] is ICollection<TService> collection)
            return collection;
#if NET9_0_OR_GREATER
        lock (_providerLock)
#else
        lock (_provider)
#endif
        {
            if (_provider[key] is ICollection<TService> collectionNew)
                return collectionNew;
            collection = CreateServiceCollection<TService>();
            _provider[key] = collection;
        }
        return collection;
    }
    /// <summary>
    /// 构造服务集合
    /// </summary>
    /// <returns></returns>
    protected virtual ICollection<TService> CreateServiceCollection<TService>()
        => [];
}
