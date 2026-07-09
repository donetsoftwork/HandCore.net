namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <param name="group"></param>
/// <param name="valueComparer"></param>
public class GroupSet(IDictionary<string, HashSet<string>> group, StringComparer valueComparer)
    : GroupSet<string, string>(group, valueComparer)
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    public GroupSet(StringComparer keyComparer, StringComparer valueComparer)
        : this(new Dictionary<string, HashSet<string>>(keyComparer), valueComparer)
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupSet()
        : this(new Dictionary<string, HashSet<string>>(StringComparer.Ordinal), StringComparer.Ordinal)
    {
    }
}
