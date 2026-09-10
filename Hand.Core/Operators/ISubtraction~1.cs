namespace Hand.Operators;

/// <summary>
/// 减法
/// </summary>
public interface ISubtraction<T>
{
    /// <summary>
    /// 减
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    T Subtract(T other);
}
