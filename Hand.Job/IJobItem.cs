using Hand.States;

namespace Hand.Job;

/// <summary>
/// 任务元素
/// </summary>
public interface IJobItem
    : IState<bool>, ICancelable, IExceptionable
{
    /// <summary>
    /// 执行
    /// </summary>
    void Run();
}
