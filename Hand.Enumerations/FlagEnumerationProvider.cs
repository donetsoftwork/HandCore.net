namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举提供者
/// </summary>
public sealed class FlagEnumerationProvider
    : FlagEnumerationProvider<FlagEnumeration>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="items"></param>
    /// <param name="flags"></param>
    /// <param name="names"></param>
    /// <param name="originals"></param>
    public FlagEnumerationProvider(List<FlagEnumeration> items, List<FlagEnumeration> flags, IDictionary<string, FlagEnumeration> names, IDictionary<long, FlagEnumeration> originals)
        : base(items, flags, names, originals)
    {
        _empty = new Lazy<FlagEnumeration>(CreateEmpty, true);
    }
    #region 配置
    /// <summary>
    /// 空枚举
    /// </summary>
    private readonly Lazy<FlagEnumeration> _empty;
    /// <inheritdoc />
    public override FlagEnumeration Empty 
        => _empty.Value;
    #endregion

    /// <inheritdoc />
    protected override FlagEnumeration CreateFlag(ISet<string> flags, long original, string description = "")
        => new(flags, original, description);
}
