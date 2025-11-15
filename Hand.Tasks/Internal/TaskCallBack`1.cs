using Hand.Job;
using Hand.States;
using System.Runtime.CompilerServices;

namespace Hand.Tasks.Internal;

/// <summary>
/// Task回调
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class TaskCallBack<TResult>
    : IJobState
    , ISuccessable
    , IFailable
    , ICancelable
    , IExceptionable
{
    #region 配置
    /// <summary>
    /// Task封装
    /// </summary>
    protected readonly TaskCompletionSource<TResult> _source = new();
    //protected readonly AsyncTaskMethodBuilder<TResult> _source = AsyncTaskMethodBuilder<TResult>.Create();
    /// <summary>
    /// Task
    /// </summary>
    public Task<TResult> Task
        => _source.Task;
    /// <inheritdoc />
    bool IJobState.IsSuccess
        => Task.IsCompleted;
    /// <inheritdoc />
    bool IJobState.IsFail
        => Task.IsCanceled;
    /// <inheritdoc />
    bool IJobState.IsCancel
        => Task.IsCanceled;
    /// <inheritdoc />
    Exception IJobState.Exception
        => Task.Exception;
    #endregion
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnSuccess(TResult result)
        => _source.SetResult(result);
    /// <inheritdoc />
    public virtual void OnFail()
        => _source.SetCanceled();
    /// <inheritdoc />
    public virtual void OnCancel()
        => _source.SetCanceled();
    /// <inheritdoc />
    public virtual void OnException(Exception exception)
        => _source.SetException(exception);
    /// <inheritdoc />
    void ISuccessable.OnSuccess()
        => OnSuccess(default);
}
