namespace Hand.Events;

/// <summary>
/// 事件派发
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// 通知
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>-+
    /// <returns></returns>
    void Notice<TEvent>(TEvent @event);
}
