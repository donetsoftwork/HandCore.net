namespace Hand.Configuration;

/// <summary>
/// 配置(一般为用户配置)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IConfigure<TKey, TValue>
{
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set(in TKey key, TValue value);
}
