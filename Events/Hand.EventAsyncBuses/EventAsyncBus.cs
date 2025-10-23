using Hand.EventHandlers;

namespace Hand.Events;

/// <summary>
/// 异步事件总线
/// </summary>
public class EventAsyncBus(
    IEventHandlerProvider handlerProvider
    , EventAsyncBusOptions options)
    : IEventAsyncBus
{
    #region 配置
    private readonly IAsyncDispatcher _dispatcher = options.TasksWhenAll
        ? new WhenAllAwaitDispatcher(handlerProvider)
        : new ForeachAwaitDispatcher(handlerProvider);
    /// <summary>
    /// 事件异步分发器
    /// </summary>
    public IAsyncDispatcher Dispatcher 
        => _dispatcher;
    #endregion
    /// <summary>
    /// 发布事件(直接分发)
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        => await _dispatcher.DispatchAsync(@event, cancellationToken);
}
