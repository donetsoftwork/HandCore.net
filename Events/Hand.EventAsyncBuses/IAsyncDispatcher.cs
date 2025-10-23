namespace Hand.Events;

/// <summary>
/// 异步事件分发器
/// </summary>
public interface IAsyncDispatcher
{
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}
