using Hand.Collections;
using Hand.Storage;

namespace Hand.Cache;

/// <summary>
/// 泛型缓存
/// </summary>
public interface IGenericCacher<TKey>
    : IGenericGet<TKey>, IStore<TKey>
{
    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool Contains(in TKey key);
    /// <summary>
    /// 尝试获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cached"></param>
    /// <returns></returns>
    bool TryGetCache<TValue>(in TKey key, out TValue cached);
}
