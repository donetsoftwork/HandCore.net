namespace Hand.Events;

/// <summary>
/// 异步事件总线
/// </summary>
public interface IEventAsyncBus
{
    /// <summary>
    /// 发布事件(直接分发)
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}
