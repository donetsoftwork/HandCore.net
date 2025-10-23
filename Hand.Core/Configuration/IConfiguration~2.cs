namespace Hand.Configuration;

/// <summary>
/// 配置(一般为用户配置)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TConfig"></typeparam>
public interface IConfiguration<TKey, TConfig>
    : IConfigure<TKey, TConfig>
{
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    bool TryGetConfig(TKey key, out TConfig config);
}
