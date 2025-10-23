namespace Hand.Events;

/// <summary>
/// 事件总线
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// 发布事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    void Publish<TEvent>(TEvent @event);
}
