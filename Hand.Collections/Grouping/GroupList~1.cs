namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="group"></param>
public class GroupList<TValue>(IDictionary<string, List<TValue>> group)
    : GroupList<string, TValue>(group)
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="comparer"></param>
    public GroupList(StringComparer comparer)
        : this(new Dictionary<string, List<TValue>>(comparer))
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupList()
        : this(new Dictionary<string, List<TValue>>(StringComparer.Ordinal))
    {
    }
}
