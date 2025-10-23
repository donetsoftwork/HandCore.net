namespace Hand.Cache;

/// <summary>
/// 
/// </summary>
public interface IGenericCacher
{
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    void SetCache<TSource, TDest>(TSource source, TDest dest);
    /// <summary>
    /// 读取缓存
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    bool TryGetCache<TSource, TDest>(TSource source, out TDest dest);
}
