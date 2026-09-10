using Hand.Comparers;
using Hand.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Hand.Enumerations;

/// <summary>
/// 位标记枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
/// <param name="flags"></param>
/// <param name="names"></param>
/// <param name="originals"></param>
public abstract class FlagEnumerationProvider<TEnumeration>(TEnumeration[] flags, IReadOnlyDictionary<string, TEnumeration> names, IReadOnlyDictionary<long, TEnumeration> originals)
    : EnumerationProvider<TEnumeration>([.. originals.Values], names, originals), IFlagEnumerationProvider<TEnumeration>
    where TEnumeration : IFlagEnumeration
{
    #region 配置
    /// <summary>
    /// 枚举分隔符
    /// </summary>
    private static readonly char[] _separator = [','];
    /// <summary>
    /// 
    /// </summary>
    private readonly IEqualityComparer<string> _comparer = CompareConverter.GetComparer(names);
    /// <summary>
    /// 空枚举原始值
    /// </summary>
    public const long EmptyOriginal = 0L;
    /// <inheritdoc cref="Flags" path="/summary"/>
    protected TEnumeration[] _flags = flags;
    /// <inheritdoc />
    public TEnumeration[] Flags
        => _flags;
    /// <summary>
    /// 空枚举
    /// </summary>
    public abstract TEnumeration Empty { get; }
    #endregion
    #region Get
    /// <inheritdoc />
    public override TEnumeration Get(string name, TEnumeration defaultValue)
    {
        var value = base.Get(name, defaultValue);
        if (value.Original == EmptyOriginal)
            return defaultValue;
        return value;
    }
    /// <inheritdoc />
    public override TEnumeration Get(long original, TEnumeration defaultValue)
    {
        var value = base.Get(original, defaultValue);
        if (value.Original == EmptyOriginal)
            return defaultValue;
        return value;
    }
    #endregion
    #region TryParse
    /// <inheritdoc />
    public bool TryParse(string name, [NotNullWhen(true)] out TEnumeration? value)
    {
        if(_names.TryGetValue(name, out value))
            return true;
        return TryParse(name.Split(_separator), out value);
    }
    /// <inheritdoc />
    public bool TryParse(long original, [NotNullWhen(true)] out TEnumeration? value)
    {
        if (_originals.TryGetValue(original, out value))
            return true;
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
        return (value = result) is not null;
    }
    #endregion
    /// <inheritdoc cref="TryParse(string, out TEnumeration)" path="/*"/>
    public bool TryParse(string[] name, out TEnumeration? value)
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
        return (value = result) is not null;
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

        var enumeration = Get(combinedOriginal);
        if (enumeration is null)
        {
            var combinedFlags = new HashSet<string>(left.Names.Except(right.Names, _comparer), _comparer);
            return CreateFlag(combinedFlags, combinedOriginal, description);
        }
        return enumeration;
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

        var enumeration = Get(combinedOriginal);
        if (enumeration is null)
        {
            var leftFlags = left.Names;
            var combinedFlags = new HashSet<string>(leftFlags, _comparer);
            foreach (var flagName in right.Names)
            {
                combinedFlags.Add(flagName);
            }
            return CreateFlag(combinedFlags, combinedOriginal, description);
        }
        return enumeration;
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