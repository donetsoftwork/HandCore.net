using Hand.EventHandlers;
using Hand.Services;

namespace Hand.Events;

/// <summary>
/// 事件处理器提供者(字典实现)
/// </summary>
public class EventHandlerDictionaryProvider
    : IEventHandlerProvider
{
    #region 配置
    /// <summary>
    /// 存储同步事件订阅
    /// </summary>
    private readonly ServiceDictionaryProvider _handlers = new();
    /// <summary>
    /// 存储异步事件订阅
    /// </summary>
    private readonly ServiceDictionaryProvider _asyncHandlers = new();
    #endregion
    /// <summary>
    /// 注册同步事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="handler"></param>
    public void AddHandler<TEvent>(IEventHandler<TEvent> handler)
        => _handlers.AddService(typeof(TEvent), handler);
    /// <summary>
    /// 注册异步事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="handler"></param>
    public void AddTaskHandler<TEvent>(ITaskEventHandler<TEvent> handler)
        => _asyncHandlers.AddService(typeof(TEvent), handler);
    #region IEventHandlerProvider
    /// <inheritdoc />
    public IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>()
        => _handlers.GetServices<IEventHandler<TEvent>>(typeof(TEvent));
    /// <inheritdoc />
    public IEnumerable<ITaskEventHandler<TEvent>> GetTaskHandlers<TEvent>()
        => _asyncHandlers.GetServices<ITaskEventHandler<TEvent>>(typeof(TEvent));
    #endregion
}
