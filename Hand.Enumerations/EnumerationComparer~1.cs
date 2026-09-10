using Hand.Primitives;

namespace Hand.Enumerations;

/// <summary>
/// 枚举比较器
/// </summary>
/// <typeparam name="TEnumeration"></typeparam>
public class EnumerationComparer<TEnumeration>
    : IEqualityComparer<TEnumeration>
    where TEnumeration : IEnumeration
{
    #region IEqualityComparer<TEnumeration>
    /// <inheritdoc />
    bool IEqualityComparer<TEnumeration>.Equals(TEnumeration? x, TEnumeration? y)
    {
        if (x is null || y is null)
            return false;
        return Equals(x, y);
    }
    #endregion
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool Equals(TEnumeration x, TEnumeration y)
        => x.Original == y.Original;
    /// <summary>
    /// 计算HashCode
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int GetHashCode(TEnumeration obj)
        => obj.Original.GetHashCode();
    /// <summary>
    /// 默认比较器
    /// </summary>
    public static EnumerationComparer<TEnumeration> Default
        => Inner.Default;

    class Inner
    {
        /// <summary>
        /// 默认比较器
        /// </summary>
        internal static readonly EnumerationComparer<TEnumeration> Default = new();
    }
}
