namespace Hand.Collections.Grouping;

/// <summary>
/// 分组列表
/// </summary>
/// <param name="group"></param>
/// <param name="valueComparer"></param>
public class GroupSet(IDictionary<string, ISet<string>> group, IEqualityComparer<string> valueComparer)
    : GroupSet<string, string>(group, valueComparer)
{
    /// <summary>
    /// 分组列表
    /// </summary>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    public GroupSet(IEqualityComparer<string> keyComparer, IEqualityComparer<string> valueComparer)
        : this(new Dictionary<string, ISet<string>>(keyComparer), valueComparer)
    {
    }
    /// <summary>
    /// 分组列表
    /// </summary>
    public GroupSet()
        : this(new Dictionary<string, ISet<string>>(StringComparer.Ordinal), StringComparer.Ordinal)
    {
    }
}
