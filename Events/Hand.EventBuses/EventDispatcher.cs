using Hand.EventHandlers;
using Hand.Tasks;

namespace Hand.Events;

/// <summary>
/// 事件分发器
/// </summary>
public class EventDispatcher(
    IEventHandlerProvider handlerProvider
    , TimeSpan handerTimeOut
    , ConcurrentTaskScheduler scheduler
    , TaskFactory taskFactory)
{
    #region 配置
    private readonly IEventHandlerProvider _handlerProvider = handlerProvider;
    private readonly ConcurrentTaskScheduler _scheduler = scheduler;
    private readonly TaskFactory _taskFactory = taskFactory;
    private readonly TimeSpan _handerTimeOut = handerTimeOut;
    #endregion
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    public void Dispatch<TEvent>(TEvent @event)
    {
        var tokenSource = new CancellationTokenSource(_handerTimeOut);
        foreach (var handler in _handlerProvider.GetTaskHandlers<TEvent>())
        {
            _taskFactory.StartNew(() => handler.TaskHandle(@event, tokenSource.Token))
                .ContinueWith(t =>
                {
                    var task = t.Result;
                    // 异步事件分发注册处理等待完成
                    if (task is not null)
                        _scheduler.AddTask(task);
                }, default, TaskContinuationOptions.OnlyOnRanToCompletion, _scheduler);
        }
        // 使用任务工厂启动同步事件分发操作
        foreach (var handler in _handlerProvider.GetHandlers<TEvent>())
            _taskFactory.StartNew(() => Handle(handler, @event, tokenSource.Token));
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
