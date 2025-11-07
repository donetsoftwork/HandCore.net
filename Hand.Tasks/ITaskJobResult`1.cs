using Hand.States;

namespace Hand.Tasks;

/// <summary>
/// 异步任务结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ITaskJobResult<TResult>
    : IState<bool>, ICancelable, IExceptionable
{
    /// <summary>
    /// Task
    /// </summary>
    Task<TResult> Task { get; }
}
