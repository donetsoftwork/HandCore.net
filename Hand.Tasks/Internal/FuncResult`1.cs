using Hand.Job;
using Hand.States;
using Hand.Structural;

namespace Hand.Tasks.Internal;

/// <summary>
/// Func结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
internal class FuncResult<TResult>(Func<TResult> original)
    : TaskCallBack<TResult>, IJobItem, IWrapper<Func<TResult>>, ITaskJobResult<TResult>
{
    #region 配置
    private readonly Func<TResult> _original = original;
    /// <inheritdoc />
    public Func<TResult> Original
        => _original;
    /// <inheritdoc />
    public bool Status
        => true;    
    #endregion
    /// <inheritdoc />
    public void Run()
    {
        OnSuccess(_original());
    }
}
