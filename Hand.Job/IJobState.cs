namespace Hand.Job;

/// <summary>
/// 任务状态
/// </summary>
public interface IJobState
{
    /// <summary>
    /// 是否执行成功
    /// </summary>
    bool IsSuccess { get; }
    /// <summary>
    /// 是否执行失败
    /// </summary>
    bool IsFail { get; }
    /// <summary>
    /// 是否取消
    /// </summary>
    bool IsCancel { get; }
    /// <summary>
    /// 异常
    /// </summary>
    public Exception Exception { get; }
}
