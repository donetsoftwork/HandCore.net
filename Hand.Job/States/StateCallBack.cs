using Hand.Job;

namespace Hand.States;

/// <summary>
/// 状态回调
/// </summary>
public class StateCallBack
    : IJobState
    , ISuccessable
    , IFailable
    , ICancelable
    , IExceptionable
{
    #region 配置
    private bool _isSuccess = false;
    private bool _isFail = false;
    private bool _isCancel = false;
    private Exception _exception;
    /// <inheritdoc />
    public bool IsSuccess
        => _isSuccess;
    /// <inheritdoc />
    public bool IsFail
        => _isFail;
    /// <inheritdoc />
    public bool IsCancel
        => _isCancel || _exception is OperationCanceledException;
    /// <inheritdoc />
    public Exception Exception
        => _exception;
    #endregion
    /// <inheritdoc />
    public virtual void OnSuccess()
        => _isSuccess = true;
    /// <inheritdoc />
    public virtual void OnFail()
        => _isFail = true;
    /// <inheritdoc />
    public virtual void OnCancel()
        => _isCancel = true;
    /// <inheritdoc />
    public virtual void OnException(Exception exception)
        => _exception = exception;
}
