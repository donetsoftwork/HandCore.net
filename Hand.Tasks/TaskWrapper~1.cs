using Hand.Structural;

namespace Hand.Tasks;

/// <summary>
/// Task包装器
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class TaskWrapper<TResult>
    : TaskWrapBase<TResult>, IWrapper<Task<TResult>>
{
    internal TaskWrapper(Func<TResult> func)
    {
        _func = func;
    }
    #region 配置
    private readonly Func<TResult> _func;
    #endregion
    /// <inheritdoc />
    protected override void RunCore()
    {
        _source.SetResult(_func());
    }
}
