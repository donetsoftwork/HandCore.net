namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="group"></param>
public class GroupList<TKey, TValue>(IDictionary<TKey, List<TValue>> group)
    : IGroupCollection<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="comparer"></param>
    public GroupList(IEqualityComparer<TKey> comparer)
        : this(new Dictionary<TKey, List<TValue>>(comparer))
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupList()
        : this(new Dictionary<TKey, List<TValue>>())
    {
    }
    #region 配置
    private readonly IDictionary<TKey, List<TValue>> _group = group;
    /// <inheritdoc />
    public IEnumerable<TKey> Keys
        => _group.Keys;
    #endregion
    /// <inheritdoc />
    public bool ContainsKey(TKey key)
        => _group.ContainsKey(key);
    /// <inheritdoc />
    public IEnumerable<TValue> GetValues(TKey key)
    {
        if (_group.TryGetValue(key, out var values))
            return values;
        return [];
    }
    /// <inheritdoc />
    public void Add(TKey key, TValue value)
        => _group.Add(key, value);
}
