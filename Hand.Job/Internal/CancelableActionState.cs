using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 包装可取消的Action
/// </summary>
/// <param name="original"></param>
/// <param name="token"></param>
public class CancelableActionState(Action original, CancellationToken token)
    : StateCallBack, IJobItem, IWrapper<Action>
{
    #region 配置
    private readonly Action _original = original;
    private readonly CancellationToken _token = token;
    /// <inheritdoc />
    public Action Original
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
        _original();
        OnSuccess();
    }
}
