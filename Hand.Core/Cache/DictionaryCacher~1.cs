using System.Collections;

namespace Hand.Cache;

/// <summary>
/// 字典缓存
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="provider"></param>
public sealed class DictionaryCacher<TKey>(IDictionary provider)
    : IGenericCacher<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 字典缓存
    /// </summary>
    public DictionaryCacher()
        : this(new Hashtable())
    {
    }
    #region 配置
    /// <summary>
    /// 存储字典
    /// </summary>
    private readonly IDictionary _provider = provider;
    #endregion
    #region ICacher<TKey, TValue>
    #region IGenericGet<TKey>
    /// <inheritdoc />
    public TValue Get<TValue>(TKey spec)
    {
        TryGetCache<TValue>(spec, out var value);
        return value;
    }
    #endregion
    /// <inheritdoc />
    public bool Contains(in TKey key)
        => _provider.Contains(key);
    /// <inheritdoc />
    public bool TryGetCache<TValue>(in TKey key, out TValue cached)
    {
        if(_provider[key] is TValue value)
        {
            cached = value;
            return true;
        }
        cached = default!;
        return false;
    }
    #region IStore<TKey, TValue>
    /// <inheritdoc />
    public void Save<TValue>(in TKey key, TValue value)
        => _provider[key] = value;
    #endregion
    #endregion
}
