using Hand.Job;
using Hand.Structural;

namespace Hand.Tasks.Internal;

/// <summary>
/// 可取消异步结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="token"></param>
internal class CancelableTaskResult<TResult>(Func<CancellationToken, Task<TResult>> original, CancellationToken token)
    : TaskCallBack<TResult>, IAsyncJobItem, IWrapper<Func<CancellationToken, Task<TResult>>>, ITaskJobResult<TResult>
{
    #region 配置
    private readonly Func<CancellationToken, Task<TResult>> _original = original;
    private readonly CancellationToken _token = token;
    /// <inheritdoc />
    public Func<CancellationToken, Task<TResult>> Original
        => _original;
    /// <summary>
    /// 取消令牌
    /// </summary>
    public CancellationToken Token
        => _token;
    /// <inheritdoc />
    public bool Status
        => !_token.IsCancellationRequested;
    #endregion
    /// <inheritdoc />
    public async Task RunAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested || _token.IsCancellationRequested)
        {
            OnCancel();
            return;
        }
        var linked = CancellationTokenSource.CreateLinkedTokenSource(_token, token);
        var task = _original(linked.Token);
        OnSuccess(await task.ConfigureAwait(false));
    }
}
