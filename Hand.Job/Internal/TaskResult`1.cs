using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 异步结果
/// </summary>
/// <param name="original"></param>
public class TaskResult<TResult>(Func<Task<TResult>> original)
    : ResultCallBack<TResult>, IAsyncJobItem, IWrapper<Func<Task<TResult>>>
{
    #region 配置
    private readonly Func<Task<TResult>> _original = original;
    /// <inheritdoc />
    public Func<Task<TResult>> Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;    
    #endregion
    /// <inheritdoc />
    public async Task RunAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            OnCancel();
            return;
        }
        var task = _original();
        OnSuccess(await task.ConfigureAwait(false));
    }
}
