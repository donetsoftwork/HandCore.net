using Hand.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Hand.Enumerations;

/// <summary>
/// 枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
/// <param name="items"></param>
/// <param name="names"></param>
/// <param name="originals"></param>
public class EnumerationProvider<TEnumeration>(List<TEnumeration> items, IDictionary<string, TEnumeration> names, IDictionary<long, TEnumeration> originals)
    : IEnumerationProvider<TEnumeration>
    where TEnumeration : IEnumeration
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="names"></param>
    public EnumerationProvider(IDictionary<string, TEnumeration> names)
        : this([], names, new Dictionary<long, TEnumeration>())
    {
        Check(names);
    }
    #region 配置
    /// <inheritdoc cref="Items" path="/summary"/>
    protected readonly List<TEnumeration> _items = items;
    /// <inheritdoc cref="Names" path="/summary"/>
    protected readonly IDictionary<string, TEnumeration> _names = names;
    /// <inheritdoc cref="Originals" path="/summary"/>
    protected readonly IDictionary<long, TEnumeration> _originals = originals;

    /// <inheritdoc />
    public IEnumerable<TEnumeration> Items
        => _items;
    /// <inheritdoc />
    public int Count
        => _items.Count;
    /// <inheritdoc />
    public IEnumerable<string> Names 
        => _names.Keys;
    /// <inheritdoc />
    public IEnumerable<long> Originals 
        => _originals.Keys;
    #endregion
    /// <summary>
    /// 添加成员
    /// </summary>
    /// <param name="names"></param>
    protected void Check(IDictionary<string, TEnumeration> names)
    {
        foreach (var item in names)
            Check(item.Key, item.Value);
    }
    /// <summary>
    /// 添加成员
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    protected virtual bool Check(string name, TEnumeration item)
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
        return true;
    }
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
        => _names.TryGetValue(name, out var value) ? value : defaultValue;
    /// <inheritdoc />
    public virtual TEnumeration Get(long original, TEnumeration defaultValue)
        => _originals.TryGetValue(original, out var value) ? value : defaultValue;
    #endregion
    #region Get
    /// <inheritdoc />
    public bool TryGet(string name, [NotNullWhen(true)] out TEnumeration? value)
        => _names.TryGetValue(name, out value);
    /// <inheritdoc />
    public bool TryGet(long original, [NotNullWhen(true)] out TEnumeration? value)
        => _originals.TryGetValue(original, out value);
    #endregion
    /// <inheritdoc />
    public TEnumeration? GetByDescription(string description)
        => _items.FirstOrDefault(item => item.Description == description);
}
