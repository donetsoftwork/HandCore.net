using Hand.EventHandlers;

namespace Hand.Events;

/// <summary>
/// 逐个等待事件分发
/// </summary>
/// <param name="handlerProvider"></param>
public class ForeachAwaitDispatcher(IEventHandlerProvider handlerProvider)
    : IAsyncDispatcher
{
    #region 配置
    private readonly IEventHandlerProvider _handlerProvider = handlerProvider;
    #endregion

    /// <inheritdoc />
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        var asyncHandlers = _handlerProvider.GetTaskHandlers<TEvent>();
        foreach (var handler in asyncHandlers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await handler.TaskHandle(@event, cancellationToken);
        }
        var handlers = _handlerProvider.GetHandlers<TEvent>();
        foreach (var handler in handlers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            handler.Handle(@event);
        }            
    }
}
