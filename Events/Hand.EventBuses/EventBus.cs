using Hand.Concurrent;
using Hand.EventHandlers;
using Hand.Job;
using Hand.Tasks;

namespace Hand.Events;

/// <summary>
/// 事件总线
/// </summary>
public class EventBus : IEventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    /// <param name="handlerProvider"></param>
    /// <param name="options"></param>
    public EventBus(IEventHandlerProvider handlerProvider, EventBusOptions options)
    {
        var concurrency = new ConcurrentControl(options.ConcurrencyLevel);
        _scheduler = new ConcurrentTaskScheduler(concurrency);
        _taskFactory = new TaskFactory(_scheduler);
        _dispatcher = new EventDispatcher(handlerProvider, options.HanderTimeOut, _scheduler, _taskFactory);
        _job = CreateJob(_scheduler, options.ReduceTime);
    }
    #region 配置
    private readonly EventDispatcher _dispatcher;
    private readonly ConcurrentTaskScheduler _scheduler;
    /// <summary>
    /// 任务工厂
    /// </summary>
    private readonly TaskFactory _taskFactory;
    /// <summary>
    /// 作业服务
    /// </summary>
    private readonly IJobService _job;
    /// <summary>
    /// 事件分发器
    /// </summary>
    public EventDispatcher Dispatcher
        => _dispatcher;
    #endregion
    /// <inheritdoc />
    public void Publish<TEvent>(TEvent @event)
    {
        // 添加分发任务
        _taskFactory.StartNew(() => _dispatcher.Dispatch(@event));
        // 启动作业执行
        _job.Start();
    }
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <param name="scheduler"></param>
    /// <param name="reduceTime"></param>
    /// <returns></returns>
    public static IJobService CreateJob(ConcurrentTaskScheduler scheduler, TimeSpan reduceTime)
    {
        var concurrencyLevel = scheduler.MaximumConcurrencyLevel;
        if (concurrencyLevel<1)
            return new ReduceJobService(scheduler, reduceTime);
        return new ConcurrentJobService(scheduler, reduceTime, concurrencyLevel);
    }
}
