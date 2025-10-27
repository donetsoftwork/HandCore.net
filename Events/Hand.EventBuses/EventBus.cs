using Hand.EventHandlers;

namespace Hand.Events;

/// <summary>
/// 事件总线
/// </summary>
/// <param name="handlerProvider"></param>
/// <param name="dispatcher"></param>
public class EventBus(IEventHandlerProvider handlerProvider, EventDispatcher dispatcher)
    : IEventBus
{
    #region 配置
    /// <summary>
    /// 事件处理器提供者
    /// </summary>
    private readonly IEventHandlerProvider _handlerProvider = handlerProvider;
    /// <summary>
    /// 事件分发器
    /// </summary>
    private readonly EventDispatcher _dispatcher = dispatcher;
    #endregion
    /// <inheritdoc />
    public void Publish<TEvent>(TEvent @event)
        => _dispatcher.Dispatch(@event, _handlerProvider);
}
