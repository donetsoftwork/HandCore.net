using Hand.States;

namespace Hand.Tasks;

/// <summary>
/// 异步任务状态
/// </summary>
public interface ITaskJobState
     : IState<bool>, ICancelable, IExceptionable
{
    /// <summary>
    /// Task
    /// </summary>
    Task Task { get; }
}