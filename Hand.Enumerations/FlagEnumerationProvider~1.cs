using Hand.Comparers;
using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
/// <param name="items"></param>
/// <param name="flags"></param>
/// <param name="names"></param>
/// <param name="originals"></param>
public abstract class FlagEnumerationProvider<TEnumeration>(List<TEnumeration> items, List<TEnumeration> flags, IDictionary<string, TEnumeration> names, IDictionary<long, TEnumeration> originals)
    : EnumerationProvider<TEnumeration>(items, names, originals), IFlagEnumerationProvider<TEnumeration>
    where TEnumeration : IFlagEnumeration
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public FlagEnumerationProvider(IDictionary<string, TEnumeration> names)
        : this([], [], names, new Dictionary<long, TEnumeration>())
    {
        Check(names);
    }
    #region 配置
    /// <summary>
    /// 枚举分隔符
    /// </summary>
    private static readonly char[] _separator = [','];
    /// <summary>
    /// 枚举名比较器
    /// </summary>
    private readonly IEqualityComparer<string> _comparer = CompareConverter.GetComparer(names);
    /// <summary>
    /// 空枚举原始值
    /// </summary>
    public const long EmptyOriginal = 0L;
    /// <inheritdoc cref="Flags" path="/summary"/>
    protected List<TEnumeration> _flags = flags;
    /// <inheritdoc />
    public IEnumerable<TEnumeration> Flags
        => _flags;
    /// <summary>
    /// 空枚举
    /// </summary>
    public abstract TEnumeration Empty { get; }
    #endregion
    /// <inheritdoc />
    protected override bool Check(string name, TEnumeration item)
    {
        var original = item.Original;
        if (_originals.TryGetValue(original, out var enumeration))
        {
            // 处理枚举别名
            _names[name] = _names[enumeration.Name] = enumeration;
            return false;
        }
        _items.Add(item);
        _originals.Add(original, item);
        if (FlagEnumeration.VerifyFlag(original))
            _flags.Add(item);
        return true;
    }
    #region Get
    /// <inheritdoc />
    public override TEnumeration Get(string name, TEnumeration defaultValue)
    {
        if(_names.TryGetValue(name, out var value))
            return value.Original == EmptyOriginal ? defaultValue : value;
        return defaultValue;
    }
    /// <inheritdoc />
    public override TEnumeration Get(long original, TEnumeration defaultValue)
    {
        if (_originals.TryGetValue(original, out var value))
            return value.Original == EmptyOriginal ? defaultValue : value;
        return defaultValue;
    }
    /// <inheritdoc />
    public virtual TEnumeration Get(string name)
        => _names.TryGetValue(name, out var value) ? value : Empty;
    /// <inheritdoc />
    public virtual TEnumeration Get(long original)
        => _originals.TryGetValue(original, out var value) ? value : Empty;
    #endregion
    #region TryParse
    /// <inheritdoc />
    public TEnumeration Parse(string name)
    {
        if(_names.TryGetValue(name, out var value))
            return value;
        return Parse(name.Split(_separator));
    }
    /// <inheritdoc />
    public TEnumeration Parse(long original)
    {
        if (_originals.TryGetValue(original, out var value))
            return value;
        TEnumeration? result = default;
        foreach (var flag in _flags)
        {
            var itemOriginal = flag.Original;
            if (itemOriginal == 0)
                continue;
            if ((itemOriginal & original) == itemOriginal)
            {
                if (result is null)
                    result = flag;
                else
                    result = Or(result, flag);
            }
        }
        return result ?? Empty;
    }
    #endregion
    /// <inheritdoc cref="Parse(string)" path="/*"/>
    public TEnumeration Parse(string[] name)
    {
        TEnumeration? result = default;
        foreach (var item in name)
        {
            if (_names.TryGetValue(item, out var enumeration))
            {
                if (result is null)
                    result = enumeration;
                else
                    result = Or(result, enumeration);
            }
        }
        return result ?? Empty;
    }
    /// <inheritdoc />
    public TEnumeration And(TEnumeration left, TEnumeration right, string description = "")
    {
        var leftOriginal = left.Original;
        if (leftOriginal == EmptyOriginal)
            return Empty;
        var rightOriginal = right.Original;
        if (rightOriginal == EmptyOriginal)
            return Empty;

        var combinedOriginal = leftOriginal & rightOriginal;
        if (combinedOriginal == EmptyOriginal)
            return Empty;
        if (combinedOriginal == leftOriginal)
            return left;
        if (combinedOriginal == rightOriginal)
            return right;
        if (_originals.TryGetValue(combinedOriginal, out var enumeration))
            return enumeration;

        var combinedFlags = new HashSet<string>(left.Names.Except(right.Names, _comparer), _comparer);
        return CreateFlag(combinedFlags, combinedOriginal, description);
    }
    /// <inheritdoc />
    public TEnumeration Or(TEnumeration left, TEnumeration right, string description = "")
    {
        var rightOriginal = right.Original;
        if (rightOriginal == EmptyOriginal)
            return left;
        var leftOriginal = left.Original;
        if (leftOriginal == EmptyOriginal)
            return right;

        var combinedOriginal = leftOriginal | rightOriginal;
        if (combinedOriginal == leftOriginal)
            return left;
        if (combinedOriginal == rightOriginal)
            return right;
        if (_originals.TryGetValue(combinedOriginal, out var enumeration))
            return enumeration;

        var leftFlags = left.Names;
        var combinedFlags = new HashSet<string>(leftFlags, _comparer);
        foreach (var flagName in right.Names)
            combinedFlags.Add(flagName);
        return CreateFlag(combinedFlags, combinedOriginal, description);
    }
    /// <summary>
    /// 构造位标记枚举
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    protected abstract TEnumeration CreateFlag(ISet<string> flags, long original, string description = "");
    /// <summary>
    /// 构造空枚举
    /// </summary>
    /// <returns></returns>
    protected TEnumeration CreateEmpty()
    {
        if (_originals.TryGetValue(EmptyOriginal, out var enumeration))
            return enumeration;
        return CreateFlag(new HashSet<string>(_comparer), EmptyOriginal);
    }
}