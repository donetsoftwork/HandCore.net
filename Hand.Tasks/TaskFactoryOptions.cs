using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// TaskFactory配置
/// </summary>
public class TaskFactoryOptions
    : ReduceOptions
{
    /// <summary>
    /// Task创建方式
    /// </summary>
    public TaskCreationOptions CreationOptions = TaskCreationOptions.None;
    /// <summary>
    /// Continue配置
    /// </summary>
    public TaskContinuationOptions ContinuationOptions = TaskContinuationOptions.None;
}
