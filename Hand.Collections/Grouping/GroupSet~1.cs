namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="group"></param>
/// <param name="valueComparer"></param>
public class GroupSet<TValue>(IDictionary<string, HashSet<TValue>> group, IEqualityComparer<TValue> valueComparer)
    : GroupSet<string, TValue>(group, valueComparer)
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    public GroupSet(StringComparer keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new Dictionary<string, HashSet<TValue>>(keyComparer), valueComparer)
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupSet()
        : this(new Dictionary<string, HashSet<TValue>>(StringComparer.Ordinal), EqualityComparer<TValue>.Default)
    {
    }
}
