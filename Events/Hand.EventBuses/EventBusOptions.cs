using Hand.Job;

namespace Hand.Events;

/// <summary>
/// 事件总线配置
/// </summary>
public class EventBusOptions
    : ReduceOptions
{
    /// <summary>
    /// 事件处理超时时间，默认1000毫秒
    /// </summary>
    public TimeSpan HanderTimeOut = TimeSpan.FromMilliseconds(1000);
}
