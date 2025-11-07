using Hand.Job;
using Hand.Structural;

namespace Hand.Tasks.Internal;

/// <summary>
/// 异步结果
/// </summary>
/// <param name="original"></param>
internal class TaskFuncResult<TResult>(Func<Task<TResult>> original)
    : TaskCallBack<TResult>, IAsyncJobItem, IWrapper<Func<Task<TResult>>>, ITaskJobResult<TResult>
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
