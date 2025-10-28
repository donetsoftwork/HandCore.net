using Hand.Creational;

namespace Hand.Job;

/// <summary>
/// 线程作业服务池
/// </summary>
/// <param name="processor"></param>
/// <param name="maxSize"></param>
public class ThreadJobPool<TItem>(IQueueProcessor<TItem> processor, int maxSize)
    : PoolBase<IJobService>(0, maxSize)
{
    #region 配置
    private readonly IQueueProcessor<TItem> _processor = processor;
    /// <summary>
    /// 种子
    /// </summary>
    private int _jobSeed = 0;
    #endregion
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        if(_actives.Count == 0)
            return;
        foreach (var active in _actives)
            active.Stop();
    }
    /// <summary>
    /// 加速
    /// </summary>
    public void Increment()
    {
        // 尝试增加线程
        var job = Get();
        if (job == null)
            return;
        job.Start();
    }
    #region PoolBase<IJobService>
    /// <inheritdoc />
    protected override IJobService CreateNew()
        => new RecycleJobService<TItem>(this, Interlocked.Increment(ref _jobSeed), _processor);
    /// <inheritdoc />
    protected override bool Clean(ref IJobService resource)
    {
        resource.Stop();
        return base.Clean(ref resource);
    }
    #endregion
}
/// <summary>
/// 可回收作业
/// </summary>
/// <param name="pool"></param>
/// <param name="id"></param>
/// <param name="processor"></param>
internal class RecycleJobService<TItem>(IPool<IJobService> pool, int id, IQueueProcessor<TItem> processor)
    : ThreadJobService<TItem>(processor)
{
    #region 配置
    private readonly IPool<IJobService> _pool = pool;
    private readonly int _id = id;
    /// <summary>
    /// 标识
    /// </summary>
    public int Id
        => _id;
    #endregion
    /// <inheritdoc />
    protected override void Run(IQueueProcessor<TItem> processor, CancellationToken token)
    {
        base.Run(processor, token);
        _pool.Return(this);
    }
}
