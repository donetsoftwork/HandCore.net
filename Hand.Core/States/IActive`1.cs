namespace Hand.States;

/// <summary>
/// 激活
/// </summary>
public interface IActive<TInstance>
{
    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    bool Activate(TInstance instance);
}
