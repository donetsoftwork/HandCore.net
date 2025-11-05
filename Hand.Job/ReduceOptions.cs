using Hand.Collections;
using Hand.Concurrent;
using Hand.ConcurrentCollections;
using Hand.States;

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
    /// <summary>
    /// 处理子元素限时
    /// </summary>
    public TimeSpan ItemLife = TimeSpan.FromHours(1);
    #region CreateJob
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="options"></param>
    /// <param name="queue"></param>
    /// <param name="processor"></param>
    /// <returns></returns>
    public static ReduceJobService<TItem> CreateJob<TItem>(ReduceOptions options, IQueue<TItem> queue, IQueueProcessor<TItem> processor)
        => new(queue, processor, options);
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="queue"></param>
    /// <param name="processor"></param>
    /// <returns></returns>
    public ReduceJobService<TItem> CreateJob<TItem>(IQueue<TItem> queue, IQueueProcessor<TItem> processor)
        => CreateJob(this, queue, processor);
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="processor"></param>
    /// <returns></returns>
    public ReduceJobService<TItem> CreateJob<TItem>(IQueueProcessor<TItem> processor)
        => CreateJob(this, new ConcurrentQueueAdapter<TItem>(), processor);
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <param name="processor"></param>
    /// <returns></returns>
    public ReduceJobService<IState<bool>> CreateJob(Processor processor)
        => CreateJob(this, processor.Queue, processor);
    #endregion
}
