namespace Hand.EventHandlers;

/// <summary>
/// 异步事件处理器
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface ITaskEventHandler<in TEvent>
{
    /// <summary>
    /// 异步处理事件
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    Task TaskHandle(TEvent @event, CancellationToken cancellationToken);
}
