using Hand.Primitives;
using System.Runtime.CompilerServices;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举类
/// </summary>
/// <param name="names"></param>
/// <param name="name"></param>
/// <param name="original"></param>
/// <param name="description"></param>
public class FlagEnumeration(ISet<string> names, string name, long original, string description = "")
    : Enumeration(name, original, description)
    , IFlagEnumeration
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="names"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    public FlagEnumeration(ISet<string> names, long original, string description = "")
        : this(names, FormatName(names), original, description)
    {
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="comparer"></param>
    /// <param name="name"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    public FlagEnumeration(IEqualityComparer<string> comparer, string name, long original, string description = "")
        : this(new HashSet<string>(comparer) { name }, name, original, description)
    {
    }
    #region 配置
    /// <inheritdoc cref="Names" path="/summary"/>
    protected readonly ISet<string> _flags = names;

    /// <inheritdoc />
    public IEnumerable<string> Names
        => _flags;
    #endregion
    /// <inheritdoc />
    public bool HasFlag(string flag)
        => _flags.Contains(flag);
    /// <inheritdoc />
    public bool HasFlag(long flag)
        => (_original & flag) == flag;
    /// <summary>
    /// 枚举名合并
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FormatName(IEnumerable<string> names)
        => string.Join(",", names);
}
