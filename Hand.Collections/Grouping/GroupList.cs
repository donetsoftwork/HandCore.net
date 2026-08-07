namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <param name="group"></param>
public class GroupList(IDictionary<string, ICollection<string>> group)
    : GroupList<string, string>(group)
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="comparer"></param>
    public GroupList(StringComparer comparer)
        : this(new Dictionary<string, ICollection<string>>(comparer))
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupList()
        : this(new Dictionary<string, ICollection<string>>(StringComparer.Ordinal))
    {
    }
}
