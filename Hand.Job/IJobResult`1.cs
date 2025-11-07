namespace Hand.Job;

/// <summary>
/// 任务执行结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IJobResult<out TResult>
    : IJobState
{
    /// <summary>
    /// 结果
    /// </summary>
    TResult Result { get; }
}
