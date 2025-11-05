using Hand.Job;
using Hand.Tasks.Internal;

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
        _concurrentScheduler = scheduler;
        _job = options.CreateJob(scheduler.Queue, scheduler);
    }
    #region 配置
    /// <summary>
    /// 调度器
    /// </summary>
    private readonly QueueTaskScheduler _concurrentScheduler;
    ///// <summary>
    ///// 异步并发控制
    ///// </summary>
    //private readonly ConcurrentControl _taskControl;
    /// <summary>
    /// 作业服务
    /// </summary>
    private readonly ReduceJobService<Task> _job;
    /// <summary>
    /// 并发调度器
    /// </summary>
    public QueueTaskScheduler ConcurrentScheduler
        => _concurrentScheduler;
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
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task StartTask(Func<Task> func)
        => TaskState.StartTask(this, func);
    //{
    //    return StartNew(() => {
    //        var task = func();
    //        _control.Increment();
    //        task.ContinueWith(t =>
    //        {
    //            _control.Decrement();
    //            // ContinueWith不能使用当前TaskScheduler,会导致死锁
    //        }, TaskScheduler.Default);
    //        //return task;
    //    });
    //}
    //private ConcurrentDictionary<Task, Task> _tracks = [];
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task<TResult> StartTask<TResult>(Func<Task<TResult>> func)
        => FuncTaskState<TResult>.StartTask(this, func);
    //{
    //    var source = new TaskCompletionSource<TResult>();
    //    StartNew(() => {
    //        var task = func();
    //        _control.Increment();
    //        var continueTask = task.ContinueWith(t =>
    //        {
    //            _control.Decrement();
    //            //_tracks.TryRemove(t, out _);
    //            if (t.IsCanceled)
    //            {
    //                source.SetCanceled();
    //            }
    //            else if (t.IsFaulted)
    //            {
    //                source.SetException(t.Exception);
    //            }
    //            else
    //            {
    //                source.SetResult(t.Result);
    //            }
    //            // ContinueWith不能使用当前TaskScheduler,可能会导致死锁
    //        }, TaskScheduler.Default);
    //        //_tracks[task] = continueTask;
    //        //await task;
    //        //return task;
    //    });
    //    return source.Task;
    //}
}
