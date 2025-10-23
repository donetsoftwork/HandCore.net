namespace Hand.Job;

/// <summary>
/// 对象(参数)操作
/// </summary>
/// <typeparam name="TArg"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IProcessor<in TArg, TResult>
{
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="arg"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool Run(TArg arg, ref TResult result);
}
