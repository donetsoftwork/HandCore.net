using Hand.EventHandlers;
using Hand.Job;
using Hand.States;

namespace Hand.Events;

/// <summary>
/// 事件分发器
/// </summary>
public class EventDispatcher
{
    /// <summary>
    /// 事件分发器
    /// </summary>
    /// <param name="options"></param>
    public EventDispatcher(EventBusOptions options)
    {
        _handerTimeOut = options.HanderTimeOut;
        _processor = new();
        _jobService = options.CreateJob(_processor);
    }
    #region 配置
    private readonly TimeSpan _handerTimeOut;
    private readonly Processor _processor;
    private readonly ReduceJobService<IState<bool>> _jobService;
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
        var token = tokenSource.Token;
        foreach (var item in provider.GetTaskHandlers<TEvent>())
        {
            var handler = item;
            _processor.AddTask((t) => handler.TaskHandle(@event, t), token);
        }
        // 使用任务工厂启动同步事件分发操作
        foreach (var item in provider.GetHandlers<TEvent>())
        {
            var handler = item;
            _processor.Add(() => handler.Handle(@event), token);
        }
    }
}
