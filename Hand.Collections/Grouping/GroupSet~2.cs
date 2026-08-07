namespace Hand.Collections.Grouping;

/// <summary>
/// 分组排重
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="original"></param>
/// <param name="valueComparer"></param>
public class GroupSet<TKey, TValue>(IDictionary<TKey, ISet<TValue>> original, IEqualityComparer<TValue> valueComparer)
    : IGroupCollection<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 分组排重
    /// </summary>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    public GroupSet(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new Dictionary<TKey, ISet<TValue>>(keyComparer), valueComparer)
    {
    }
    /// <summary>
    /// 分组排重
    /// </summary>
    public GroupSet()
        : this(new Dictionary<TKey, ISet<TValue>>(), EqualityComparer<TValue>.Default)
    {
    }
    #region 配置
    private readonly IDictionary<TKey, ISet<TValue>> _original = original;
    private readonly IEqualityComparer<TValue> _valueComparer = valueComparer;

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
        => _original.Add(key, value, _valueComparer);
}
