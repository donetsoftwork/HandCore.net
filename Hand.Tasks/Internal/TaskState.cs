using Hand.States;
using Hand.Structural;

namespace Hand.Tasks.Internal;

/// <summary>
/// 包装Task
/// </summary>
/// <param name="original"></param>
internal class TaskState(Task original)
    : IState<bool>, IWrapper<Task>
{
    #region 配置
    private readonly Task _original = original;
    /// <inheritdoc />
    public Task Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;
    #endregion
}
