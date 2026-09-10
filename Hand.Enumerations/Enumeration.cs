using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 枚举类
/// </summary>
public class Enumeration(string name, long original, string description = "")
    : IEnumeration
{
    /// <inheritdoc cref="Name" path="/summary"/>
    protected readonly string _name = name;
    /// <inheritdoc cref="Original" path="/summary"/>
    protected readonly long _original = original;
    /// <inheritdoc cref="Description" path="/summary"/>
    protected readonly string _description = description;

    /// <inheritdoc />
    public string Name=> _name;
    /// <summary>
    /// 枚举值
    /// </summary>
    public long Original => _original;
    /// <inheritdoc />
    public string Description => _description;
}