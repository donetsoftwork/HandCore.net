using Hand.Collections;
using Hand.Creational;

namespace Hand.Job;

/// <summary>
/// 线程作业服务池
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="queue"></param>
/// <param name="processor"></param>
/// <param name="maxSize"></param>
public class ThreadJobPool<TItem>(IQueue<TItem> queue, IQueueProcessor<TItem> processor, int maxSize)
    : PoolBase<ThreadJobService<TItem>>(0, maxSize)
{
    #region 配置
    /// <summary>
    /// 队列
    /// </summary>
    private readonly IQueue<TItem> _queue = queue;
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
    protected override ThreadJobService<TItem> CreateNew()
        => new RecycleJobService<TItem>(this, Interlocked.Increment(ref _jobSeed), _queue, _processor);
    /// <inheritdoc />
    protected override bool Clean(ref ThreadJobService<TItem> resource)
    {
        resource.Stop();
        return base.Clean(ref resource);
    }
    #endregion
}
/// <summary>
/// 可回收作业
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="pool"></param>
/// <param name="id"></param>
/// <param name="queue"></param>
/// <param name="processor"></param>
internal class RecycleJobService<TItem>(ThreadJobPool<TItem> pool, int id, IQueue<TItem> queue, IQueueProcessor<TItem> processor)
    : ThreadJobService<TItem>(queue, processor)
{
    #region 配置
    private readonly ThreadJobPool<TItem> _pool = pool;
    private readonly int _id = id;
    /// <summary>
    /// 标识
    /// </summary>
    public int Id
        => _id;
    #endregion
    /// <inheritdoc />
    public override bool Activate(TItem instance)
    {
        if(base.Activate(instance))
        {
            _pool.Increment();
            return true;
        }
        return false;
    }
    /// <inheritdoc />
    public override void Dispose()
    {
        _pool.Return(this);
    }
}
