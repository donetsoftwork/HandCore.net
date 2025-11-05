using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 包装可取消异步
/// </summary>
/// <param name="original"></param>
public class TaskState(Func<Task> original)
    : StateCallBack, IAsyncJobItem, IWrapper<Func<Task>>
{
    #region 配置
    private readonly Func<Task> _original = original;
    /// <inheritdoc />
    public Func<Task> Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;
    #endregion
    /// <inheritdoc />
    public async Task RunAsync(CancellationToken token)
    {
        if(token.IsCancellationRequested)
            return;
        var task = _original();
        await task.ConfigureAwait(false);
        OnSuccess();
    }
}
