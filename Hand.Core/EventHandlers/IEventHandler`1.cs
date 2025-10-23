namespace Hand.EventHandlers;

/// <summary>
/// 事件处理器
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent>
{
    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="event"></param>
    void Handle(TEvent @event);
}
