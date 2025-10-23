namespace Hand.Job;

/// <summary>
/// 对象操作
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IProcessor<TInstance>
{
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    bool Run(ref TInstance instance);
}
