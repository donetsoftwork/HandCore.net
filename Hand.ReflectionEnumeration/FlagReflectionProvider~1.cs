using Hand.Primitives;
using System.Reflection;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举反射提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
public sealed class FlagReflectionProvider<TEnumeration>
    : FlagEnumerationProvider<TEnumeration>
    where TEnumeration : IFlagEnumeration
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="items"></param>
    /// <param name="flags"></param>
    /// <param name="names"></param>
    /// <param name="originals"></param>
    public FlagReflectionProvider(List<TEnumeration> items, List<TEnumeration> flags, IDictionary<string, TEnumeration> names, IDictionary<long, TEnumeration> originals)
        : base(items, flags, names, originals)
    {
        _empty = new Lazy<TEnumeration>(CreateEmpty, true);
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    public FlagReflectionProvider(IDictionary<string, TEnumeration> names)
        : this([], [], names, new Dictionary<long, TEnumeration>())
    {
        Check(names);
    }
    #region 配置
    /// <summary>
    /// 构造函数(参数类型: ISet[string], long, string)
    /// </summary>
    private static readonly ConstructorInfo? _constructor = GetConstructor();
    /// <summary>
    /// 空枚举
    /// </summary>
    private readonly Lazy<TEnumeration> _empty;
    /// <inheritdoc />
    public override TEnumeration Empty
        => _empty.Value;
    #endregion
    /// <inheritdoc />
    protected override TEnumeration CreateFlag(ISet<string> flags, long original, string description = "")
    {        
        var constructor = _constructor ?? throw new NotSupportedException($"{typeof(TEnumeration).FullName}缺少参数(ISet<string>, long, string)的构造函数");
        return (TEnumeration)constructor.Invoke([flags, original, description]);
    }
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <returns></returns>
    public static ConstructorInfo? GetConstructor()
        => typeof(TEnumeration).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, [typeof(ISet<string>), typeof(long), typeof(string)], null);
}
