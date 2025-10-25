namespace Hand.Tasks;

/// <summary>
/// Task包装器基类
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class TaskWrapBase<TResult>
{
    /// <summary>
    /// Task状态
    /// </summary>
    protected readonly TaskCompletionSource<TResult> _source = new();
    /// <summary>
    /// 原始Task
    /// </summary>
    public Task<TResult> Original
        => _source.Task;
    /// <summary>
    /// 实际执行逻辑
    /// </summary>
    protected abstract void RunCore();
    /// <summary>
    /// 执行
    /// </summary>
    public void Run()
    {
        try
        {
            RunCore();
        }
        catch (Exception ex)
        {
            _source.SetException(ex);
        }
    }
    /// <summary>
    /// 取消执行
    /// </summary>
    public void Cancel()
    {
        _source.SetCanceled();
    }
}
