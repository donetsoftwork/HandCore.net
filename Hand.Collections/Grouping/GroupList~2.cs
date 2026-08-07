namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="original"></param>
public class GroupList<TKey, TValue>(IDictionary<TKey, ICollection<TValue>> original)
    : IGroupCollection<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="comparer"></param>
    public GroupList(IEqualityComparer<TKey> comparer)
        : this(new Dictionary<TKey, ICollection<TValue>>(comparer))
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupList()
        : this(new Dictionary<TKey, ICollection<TValue>>())
    {
    }
    #region 配置
    private readonly IDictionary<TKey, ICollection<TValue>> _original = original;
    /// <inheritdoc />
    public IEnumerable<TKey> Keys
        => _original.Keys;
    #endregion
    /// <inheritdoc />
    public bool ContainsKey(TKey key)
        => _original.ContainsKey(key);
    /// <inheritdoc />
    public IEnumerable<TValue> GetValues(TKey key)
    {
        if (_original.TryGetValue(key, out var values))
            return values;
        return [];
    }
    /// <inheritdoc />
    public virtual void Add(TKey key, TValue value)
        => _original.Add(key, value);
}
