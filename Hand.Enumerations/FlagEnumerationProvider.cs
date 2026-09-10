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
    /// <param name="flags"></param>
    /// <param name="names"></param>
    /// <param name="originals"></param>
    public FlagEnumerationProvider(FlagEnumeration[] flags, IReadOnlyDictionary<string, FlagEnumeration> names, IReadOnlyDictionary<long, FlagEnumeration> originals)
        : base(flags, names, originals)
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
