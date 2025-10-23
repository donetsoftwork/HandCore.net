using Hand.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hand.Events;

/// <summary>
/// 事件订阅提供者(容器实现)
/// </summary>
/// <param name="provider"></param>
public class EventHandlerServiceProvider(IServiceProvider provider)
    : IEventHandlerProvider
{
    #region 配置
    /// <summary>
    /// IOC容器
    /// </summary>
    private readonly IServiceProvider _provider = provider;
    #endregion

    #region IEventHandlerProvider
    /// <summary>
    /// 获取异步事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public IEnumerable<ITaskEventHandler<TEvent>> GetTaskHandlers<TEvent>()
        where TEvent : notnull
    {
        return _provider.GetServices<ITaskEventHandler<TEvent>>();
    }
    /// <summary>
    /// 获取事件处理器
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>()
        where TEvent : notnull
    {
        return _provider.GetServices<IEventHandler<TEvent>>();
    }
    #endregion
}