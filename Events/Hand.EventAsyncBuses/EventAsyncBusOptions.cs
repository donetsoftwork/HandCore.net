namespace Hand.Events;

/// <summary>
/// 异步事件总线配置
/// </summary>
public class EventAsyncBusOptions
{
    /// <summary>
    /// 一次性等待所有任务完成
    /// </summary>
    public bool TasksWhenAll = false;
}
