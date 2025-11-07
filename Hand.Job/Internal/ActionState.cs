using Hand.States;
using Hand.Structural;

namespace Hand.Job.Internal;

/// <summary>
/// 包装Action
/// </summary>
/// <param name="original"></param>
internal class ActionState(Action original)
    : StateCallBack, IJobItem, IWrapper<Action>
{
    #region 配置
    private readonly Action _original = original;
    /// <inheritdoc />
    public Action Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;
    #endregion
    /// <inheritdoc />
    public void Run()
    {
        _original();
        OnSuccess();
    }
}
