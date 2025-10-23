namespace Hand.EventHandlers;

/// <summary>
/// 事件处理器提供者
/// </summary>
public interface IEventHandlerProvider
{
    /// <summary>
    /// 获取事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>();
    /// <summary>
    /// 获取异步事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IEnumerable<ITaskEventHandler<TEvent>> GetTaskHandlers<TEvent>();
}
