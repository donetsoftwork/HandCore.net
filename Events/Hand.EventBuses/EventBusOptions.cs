namespace Hand.Events;

/// <summary>
/// 事件总线配置
/// </summary>
public class EventBusOptions
{
    /// <summary>
    /// 事件总线并发上限，默认ushort.MaxValue
    /// </summary>
    public uint ConcurrencyLevel { get; set; } = ushort.MaxValue;
    /// <summary>
    /// 主线程休眠暂停时间，默认50毫秒
    /// </summary>
    public TimeSpan ReduceTime { get; set; } = TimeSpan.FromMilliseconds(50);
    /// <summary>
    /// 事件处理超时时间，默认1000毫秒
    /// </summary>
    public TimeSpan HanderTimeOut { get; set; } = TimeSpan.FromMilliseconds(1000);
}
