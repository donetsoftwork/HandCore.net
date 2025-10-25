using Hand.Models;
using Hand.Structural;

namespace Hand.Tasks;

/// <summary>
/// Task包装器
/// </summary>
public class TaskWrapper
    : TaskWrapBase<Empty>, IWrapper<Task>
{
    private TaskWrapper(Action action)
    {
        _action = action;
    }
    #region 配置
    private readonly Action _action;
    /// <summary>
    /// 原始Task
    /// </summary>
    public new Task Original
        => _source.Task;
    #endregion
    /// <inheritdoc />
    protected override void RunCore()
    {
        _action();
        _source.SetResult(Empty.Value);
    }
    #region Wrap
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static TaskWrapper Wrap(Action action)
        => new(action);
    /// <summary>
    /// 包装为Task
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TaskWrapper<TResult> Wrap<TResult>(Func<TResult> func)
        => new(func);
    #endregion
}
