namespace Hand.Configuration;

/// <summary>
/// 配置(一般为用户配置)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TConfig"></typeparam>
public interface IConfigure<TKey, TConfig>
{
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config"></param>
    void Set(in TKey key, TConfig config);
}
