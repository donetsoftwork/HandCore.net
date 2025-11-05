using Hand.States;

namespace Hand.Job;

/// <summary>
/// 异步任务元素
/// </summary>
public interface IAsyncJobItem
    : IState<bool>, ICancelable, IExceptionable
{
    /// <summary>
    /// 异步执行
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task RunAsync(CancellationToken token);
}
