using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 可取消异步状态
/// </summary>
/// <param name="original"></param>
/// <param name="token"></param>
public class CancelableTaskState(Func<CancellationToken, Task> original, CancellationToken token)
    : StateCallBack, IAsyncJobItem, IWrapper<Func<CancellationToken, Task>>
{
    #region 配置
    private readonly Func<CancellationToken, Task> _original = original;
    private readonly CancellationToken _token = token;
    /// <inheritdoc />
    public Func<CancellationToken, Task> Original 
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
        await task.ConfigureAwait(false);
        OnSuccess();
    }
}
