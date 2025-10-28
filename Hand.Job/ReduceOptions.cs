using Hand.Concurrent;

namespace Hand.Job;

/// <summary>
/// 节约作业服务配置
/// </summary>
public class ReduceOptions
    : ConcurrentOptions
{
    /// <summary>
    /// 主线程休眠暂停时间，默认50毫秒
    /// </summary>
    public TimeSpan ReduceTime = TimeSpan.FromMilliseconds(50);
    /// <summary>
    /// 是否自动启动
    /// </summary>
    public bool AutoStart = true;

    #region CreateJob
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="options"></param>
    /// <param name="processor"></param>
    /// <returns></returns>
    public static ReduceJobService<TItem> CreateJob<TItem>(ReduceOptions options, IQueueProcessor<TItem> processor)
    {
        var concurrencyLevel = options.ConcurrencyLevel;
        if (concurrencyLevel < 1u)
            return new ReduceJobService<TItem>(processor, options);
        return new ConcurrentJobService<TItem>(processor, options);
    }
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="processor"></param>
    /// <returns></returns>
    public ReduceJobService<TItem> CreateJob<TItem>(IQueueProcessor<TItem> processor)
        => CreateJob(this, processor);
    #endregion
}
