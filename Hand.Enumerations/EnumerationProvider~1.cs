using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
/// <param name="items"></param>
/// <param name="names"></param>
/// <param name="originals"></param>
public class EnumerationProvider<TEnumeration>(TEnumeration[] items, IReadOnlyDictionary<string, TEnumeration> names, IReadOnlyDictionary<long, TEnumeration> originals)
    : IEnumerationProvider<TEnumeration>
    where TEnumeration : IEnumeration
{
    #region 配置
    /// <inheritdoc cref="Items" path="/summary"/>
    protected readonly TEnumeration[] _items = items;
    /// <inheritdoc cref="Names" path="/summary"/>
    protected readonly IReadOnlyDictionary<string, TEnumeration> _names = names;
    /// <inheritdoc cref="Originals" path="/summary"/>
    protected readonly IReadOnlyDictionary<long, TEnumeration> _originals = originals;

    /// <inheritdoc />
    public TEnumeration[] Items
        => _items;
    /// <inheritdoc />
    public int Count
        => _items.Length;
    /// <inheritdoc />
    public IEnumerable<string> Names 
        => _names.Keys;
    /// <inheritdoc />
    public IEnumerable<long> Originals 
        => _originals.Keys;
    #endregion
    #region IsDefined
    /// <inheritdoc />
    public bool IsDefined(TEnumeration flag)
        => IsDefined(flag.Original);
    /// <inheritdoc />
    public virtual bool IsDefined(long original)
        => _originals.ContainsKey(original);
    /// <inheritdoc />
    public virtual bool IsDefined(string name)
        => _names.ContainsKey(name);
    #endregion
    #region Get
    /// <inheritdoc />
    public virtual TEnumeration Get(string name, TEnumeration defaultValue)
        => _names.GetValueOrDefault(name, defaultValue);
    /// <inheritdoc />
    public TEnumeration? Get(string name)
    {
        _names.TryGetValue(name, out var enumeration);
        return enumeration;
    }
    /// <inheritdoc />
    public virtual TEnumeration Get(long original, TEnumeration defaultValue)
        => _originals.GetValueOrDefault(original, defaultValue);
    /// <inheritdoc />
    public TEnumeration? Get(long original)
    {
        _originals.TryGetValue(original, out var enumeration);
        return enumeration;
    }
    #endregion
    /// <inheritdoc />
    public TEnumeration? GetByDescription(string description)
        => _items.FirstOrDefault(item => item.Description == description);
}
