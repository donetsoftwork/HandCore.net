using System.Diagnostics.CodeAnalysis;

namespace Hand.Configuration;

/// <summary>
/// 配置(一般为用户配置)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IConfiguration<TKey, TValue>
    : IConfigure<TKey, TValue>
{
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGetConfig(TKey key, [NotNullWhen(true)] out TValue? value);
}
