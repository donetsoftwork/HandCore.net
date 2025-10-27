using Hand.EventHandlers;
using Hand.Tasks;

namespace Hand.Events;

/// <summary>
/// 事件分发器
/// </summary>
public class EventDispatcher(EventBusOptions options)
    : ConcurrentTaskFactory(options)
{
    #region 配置
    private readonly TimeSpan _handerTimeOut = options.HanderTimeOut;
    #endregion
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="provider"></param>
    public void Dispatch<TEvent>(TEvent @event, IEventHandlerProvider provider)
    {
        var tokenSource = new CancellationTokenSource(_handerTimeOut);
        foreach (var handler in provider.GetTaskHandlers<TEvent>())
        {
            StartTask(() => handler.TaskHandle(@event, tokenSource.Token));
        }
        // 使用任务工厂启动同步事件分发操作
        foreach (var handler in provider.GetHandlers<TEvent>())
            StartNew(() => Handle(handler, @event, tokenSource.Token));
        //_job.Start();
    }
    /// <summary>
    /// 处理同步事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="handler"></param>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    private static void Handle<TEvent>(IEventHandler<TEvent> handler, TEvent @event, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        handler.Handle(@event);
    }
}
