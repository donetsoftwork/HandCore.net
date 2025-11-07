using Hand.Collections;
using Hand.Job;
using Hand.Tasks.Internal;
using TaskItem = Hand.States.IState<bool>;

namespace Hand.Tasks;

/// <summary>
/// 并发任务工厂
/// </summary>
public class ConcurrentTaskFactory
    : TaskFactory
{
    /// <summary>
    /// 并发任务工厂
    /// </summary>
    /// <param name="options"></param>
    public ConcurrentTaskFactory(TaskFactoryOptions options)
        : this(new QueueTaskScheduler(), options)
    {
    }
    /// <summary>
    /// 并发任务工厂
    /// </summary>
    /// <param name="scheduler"></param>
    /// <param name="options"></param>
    private ConcurrentTaskFactory(QueueTaskScheduler scheduler, TaskFactoryOptions options)
        : base(default, options.CreationOptions, options.ContinuationOptions, scheduler)
    {
        _queue = scheduler.Queue;
        _job = options.CreateJob(scheduler.Queue, scheduler);
    }
    #region 配置
    private readonly IQueue<TaskItem> _queue;
    /// <summary>
    /// 作业服务
    /// </summary>
    private readonly ReduceJobService<TaskItem> _job;
    /// <summary>
    /// 作业服务
    /// </summary>
    public ReduceJobService<TaskItem> Job 
        => _job;
    #endregion
    /// <summary>
    /// 启动服务
    /// </summary>
    /// <returns></returns>
    public bool Start()
        => _job.Start();
    /// <summary>
    /// 停止服务
    /// </summary>
    /// <returns></returns>
    public bool Stop()
        => _job.Stop();
    #region StartNew
    #region State
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public new Task StartNew(Action action)
    {
        var state = TaskWrapper.Wrap(action);
        _queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public new Task StartNew(Action action, CancellationToken token)
    {
        var state = TaskWrapper.Wrap(action, token);
        Processor.Enqueue(_queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public new Task<TResult> StartNew<TResult>(Func<TResult> func)
    {
        var result = TaskWrapper.Wrap(func);
        _queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public new Task<TResult> StartNew<TResult>(Func<TResult> func, CancellationToken token)
    {
        var result = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(_queue, result);
        return result.Task;
    }
    #endregion
    #endregion
    #region StartTask
    #region State
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task StartTask(Func<Task> func)
    {
        var state = TaskWrapper.Wrap(func);
        _queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="func"></param>
    /// <param name="token"></param>
    public Task StartTask(Func<CancellationToken, Task> func, CancellationToken token)
    {
        var state = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(_queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task<TResult> StartTask<TResult>(Func<Task<TResult>> func)
    {
        var result = TaskWrapper.Wrap(func);
        _queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<TResult> StartTask<TResult>(Func<CancellationToken, Task<TResult>> func, CancellationToken token)
    {
        var result = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(_queue, result);
        return result.Task;
    }
    #endregion
    #endregion
}
