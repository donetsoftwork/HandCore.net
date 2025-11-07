using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 可取消的Func的结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="token"></param>
internal class CancelableFuncResult<TResult>(Func<TResult> original, CancellationToken token)
    : ResultCallBack<TResult>, IJobItem, IWrapper<Func<TResult>>
{
    #region 配置
    private readonly Func<TResult> _original = original;
    private readonly CancellationToken _token = token;
    /// <inheritdoc />
    public Func<TResult> Original
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
    public void Run()
    {
        if (_token.IsCancellationRequested)
        {
            OnCancel();
            return;
        }
        OnSuccess(_original());
    }
}
