using Hand.Concurrent;
using Hand.Job;

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
    public ConcurrentTaskFactory(ReduceOptions options)
        : this(new ConcurrentTaskScheduler(options), options)
    {
    }
    /// <summary>
    /// 并发任务工厂
    /// </summary>
    /// <param name="scheduler"></param>
    /// <param name="options"></param>
    private ConcurrentTaskFactory(ConcurrentTaskScheduler scheduler, ReduceOptions options)
        : base(scheduler)
    {
        _control = scheduler.Control;
        _job = options.CreateJob(scheduler);
    }
    #region 配置
    /// <summary>
    /// 并发控制
    /// </summary>
    private readonly ConcurrentControl _control;
    /// <summary>
    /// 作业服务
    /// </summary>
    private readonly ReduceJobService<Task> _job;
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
    {
        return StartNew(() => {
            var task = func();
            _control.Increment();
            task.ContinueWith(t =>
            {
                _control.Decrement();
                // ContinueWith不能使用当前TaskScheduler,会导致死锁
            }, TaskScheduler.Default);
            //return task;
        });
    }
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task<TResult> StartTask<TResult>(Func<Task<TResult>> func)
    {
        var source = new TaskCompletionSource<TResult>();
        StartNew(() => {
            var task = func();
            _control.Increment();
            task.ContinueWith(t =>
            {
                _control.Decrement();
                if (t.IsCanceled)
                {
                    source.SetCanceled();
                }
                else if (t.IsFaulted)
                {
                    source.SetException(t.Exception);
                }
                else
                {
                    source.SetResult(t.Result);
                }
                // ContinueWith不能使用当前TaskScheduler,可能会导致死锁
            }, TaskScheduler.Default);
            return task;
        });
        return source.Task;
    }
}
