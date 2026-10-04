using Hand.Configuration;

namespace Hand;

/// <summary>
/// 配置扩展方法
/// </summary>
public static partial class HandCoreServices
{
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TValue GetConfig<TKey, TValue>(this IConfiguration<TKey, TValue> configuration, TKey key, TValue defaultValue)
        => configuration.TryGetConfig(key, out var value) ? value : defaultValue;
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue? GetConfig<TKey, TValue>(this IConfiguration<TKey, TValue> configuration, TKey key)
    {
        configuration.TryGetConfig(key, out var value);
        return value;
    }
}
