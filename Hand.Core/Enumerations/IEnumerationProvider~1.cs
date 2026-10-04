using Hand.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Hand.Enumerations;

/// <summary>
/// 枚举提供者
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
public interface IEnumerationProvider<TEnumeration>
    where TEnumeration : IEnumeration
{
    /// <summary>
    /// 所有枚举项
    /// </summary>
    IEnumerable<TEnumeration> Items { get; }
    /// <summary>
    /// 枚举项数
    /// </summary>
    int Count { get; }
    /// <summary>
    /// 枚举名
    /// </summary>
    IEnumerable<string> Names { get; }
    /// <summary>
    /// 枚举原始值
    /// </summary>
    IEnumerable<long> Originals { get; }
    #region IsDefined
    /// <summary>
    /// 判断是否包含枚举
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    bool IsDefined(TEnumeration flag);
    /// <summary>
    /// 判断是否包含枚举原始值
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    bool IsDefined(long original);
    /// <summary>
    /// 判断是否包含枚举标识
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool IsDefined(string name);
    #endregion
    #region Get
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    TEnumeration Get(string name, TEnumeration defaultValue);
    /// <summary>
    /// 获取枚举
    /// </summary>
    /// <param name="original"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    TEnumeration Get(long original, TEnumeration defaultValue);
    #endregion
    #region TryGet
    /// <summary>
    /// 尝试获取枚举
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGet(string name, [NotNullWhen(true)] out TEnumeration? value);
    /// <summary>
    /// 尝试获取枚举
    /// </summary>
    /// <param name="original"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGet(long original, [NotNullWhen(true)] out TEnumeration? value);
    #endregion
    /// <summary>
    /// 按备注获取枚举
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    TEnumeration? GetByDescription(string description);
}
