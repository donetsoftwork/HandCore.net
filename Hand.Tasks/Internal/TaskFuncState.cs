using Hand.Job;
using Hand.Models;
using Hand.Structural;

namespace Hand.Tasks.Internal;

/// <summary>
/// 包装可取消异步
/// </summary>
/// <param name="original"></param>
internal class TaskFuncState(Func<Task> original)
    : TaskCallBack<Empty>, IAsyncJobItem, IWrapper<Func<Task>>, ITaskJobState
{
    #region 配置
    private readonly Func<Task> _original = original;
    /// <inheritdoc />
    public Func<Task> Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;
    /// <inheritdoc />
    Task ITaskJobState.Task
        => _source.Task;
    #endregion
    /// <inheritdoc />
    public async Task RunAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return;
        var task = _original();
        await task.ConfigureAwait(false);
        OnSuccess(Empty.Value);
    }
}
