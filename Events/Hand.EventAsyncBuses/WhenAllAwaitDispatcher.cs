using Hand.EventHandlers;

namespace Hand.Events;

/// <summary>
/// 等待所有完成事件分发
/// </summary>
/// <param name="handlerProvider"></param>
public class WhenAllAwaitDispatcher(IEventHandlerProvider handlerProvider)
    : IAsyncDispatcher
{
    #region 配置
    private readonly IEventHandlerProvider _handlerProvider = handlerProvider;
    #endregion

    /// <inheritdoc />
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        var tasks = DispatchAsync(_handlerProvider, @event, cancellationToken)
            .ToArray();
        var handlers = _handlerProvider.GetHandlers<TEvent>();
        foreach (var handler in handlers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            handler.Handle(@event);
        }
        cancellationToken.ThrowIfCancellationRequested();
        await Task.WhenAll(tasks);
    }
    /// <summary>
    /// 构造分发异步操作
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="asyncHandlerProvider"></param>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static IEnumerable<Task> DispatchAsync<TEvent>(IEventHandlerProvider asyncHandlerProvider, TEvent @event, CancellationToken cancellationToken)
    {
        var asyncHandlers = asyncHandlerProvider.GetTaskHandlers<TEvent>();
        foreach (var handler in asyncHandlers)
            yield return handler.TaskHandle(@event, cancellationToken);
    }
}
