namespace Hand.Collections;

/// <summary>
/// 存储(系统行为)
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IStore<TKey>
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Save<TValue>(in TKey key, TValue value);
}
