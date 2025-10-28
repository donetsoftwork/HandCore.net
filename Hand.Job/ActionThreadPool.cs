using System.Collections.Concurrent;

namespace Hand.Job;

/// <summary>
/// Action线程池
/// </summary>
public class ActionThreadPool
    : IJobService, IQueueProcessor<Action>
{
    /// <summary>
    /// Action线程池
    /// </summary>
    /// <param name="options"></param>
    public ActionThreadPool(ReduceOptions options)
    {
        _job = options.CreateJob(this);
    }
    #region 配置
    private readonly ConcurrentQueue<Action> _actions = new();
    /// <summary>
    /// 作业服务
    /// </summary>
    private readonly ReduceJobService<Action> _job;
    #endregion
    /// <summary>
    /// 添加进队列
    /// </summary>
    /// <param name="action"></param>
    public void Add(Action action)
    {
        _actions.Enqueue(action);
    }
    #region IJobService
    /// <inheritdoc />
    public bool Start()
        => _job.Start();
    /// <inheritdoc />
    public bool Stop()
        => _job.Stop();
    /// <inheritdoc />
    void IJobService.Run()
        => _job.Run();
    #endregion
    #region ICollectionProcessor<Action>
    /// <inheritdoc />
    public bool TryTake(out Action item)
        => _actions.TryDequeue(out item);
    /// <inheritdoc />
    public bool Run(ref Action instance)
    {
        instance();
        return true;
    }
    #endregion
}
