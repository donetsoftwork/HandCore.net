namespace Hand.Collections.Grouping;

/// <summary>
/// 分组排重
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="group"></param>
/// <param name="valueComparer"></param>
public class GroupSet<TKey, TValue>(IDictionary<TKey, HashSet<TValue>> group, IEqualityComparer<TValue> valueComparer)
    : IGroupCollection<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 分组排重
    /// </summary>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    public GroupSet(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new Dictionary<TKey, HashSet<TValue>>(keyComparer), valueComparer)
    {
    }
    /// <summary>
    /// 分组排重
    /// </summary>
    public GroupSet()
        : this(new Dictionary<TKey, HashSet<TValue>>(), EqualityComparer<TValue>.Default)
    {
    }
    #region 配置
    private readonly IDictionary<TKey, HashSet<TValue>> _group = group;
    private readonly IEqualityComparer<TValue> _valueComparer = valueComparer;

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
        => _group.Add(key, value, _valueComparer);
}
