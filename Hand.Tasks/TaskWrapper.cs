using Hand.Job;
using Hand.Models;
using Hand.Structural;
using Hand.Tasks.Internal;

namespace Hand.Tasks;

/// <summary>
/// Task包装器
/// </summary>
public class TaskWrapper(Action original)
    : TaskCallBack<Empty>, IJobItem, IWrapper<Action>, ITaskJobState
{
    #region 配置
    private readonly Action _original = original;
    /// <inheritdoc />
    public Action Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;
    /// <inheritdoc />
    Task ITaskJobState.Task
        => _source.Task;
    #endregion
    /// <inheritdoc />
    public void Run()
    {
        _original();
        OnSuccess(Empty.Value);
    }
    #region 同步包装
    #region State
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static ITaskJobState Wrap(Action action)
        => new TaskWrapper(action);
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static ITaskJobState Wrap(Action action, CancellationToken token)
        => new CancelableActionState(action, token);
    #endregion
    #region Result
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static ITaskJobResult<TResult> Wrap<TResult>(Func<TResult> func)
        => new FuncResult<TResult>(func);
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static ITaskJobResult<TResult> Wrap<TResult>(Func<TResult> func, CancellationToken token)
        => new CancelableFuncResult<TResult>(func, token);
    #endregion
    #endregion
    #region 异步包装
    #region State
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static ITaskJobState Wrap(Func<Task> func)
        => new TaskFuncState(func);
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static ITaskJobState Wrap(Func<CancellationToken, Task> func, CancellationToken token)
        => new CancelableTaskState(func, token);
    #endregion
    #region Result
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static ITaskJobResult<TResult> Wrap<TResult>(Func<Task<TResult>> func)
        => new TaskFuncResult<TResult>(func);
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static ITaskJobResult<TResult> Wrap<TResult>(Func<CancellationToken, Task<TResult>> func, CancellationToken token)
        => new CancelableTaskResult<TResult>(func, token);
    #endregion
    #endregion
}
