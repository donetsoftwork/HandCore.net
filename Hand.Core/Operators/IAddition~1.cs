namespace Hand.Operators;

/// <summary>
/// 加法
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAddition<T>
{
    /// <summary>
    /// 减
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    T Add(T other);
}
