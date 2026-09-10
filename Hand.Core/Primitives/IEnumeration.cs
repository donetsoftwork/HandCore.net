using Hand.Structural;

namespace Hand.Primitives;

/// <summary>
/// 枚举接口
/// </summary>
public interface IEnumeration : IWrapper<long>
{
    /// <summary>
    /// 枚举名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 备注
    /// </summary>
    string Description { get; }
}
