using Hand.Collections;
using Hand.States;

namespace Hand.Job;

/// <summary>
/// 节约作业服务
/// </summary>
public class ReduceJobService<TItem>
    : ThreadJobService<TItem>
{
    /// <summary>
    /// 节约作业服务
    /// </summary>
    /// <param name="queue">队列</param>
    /// <param name="processor">操作</param>
    /// <param name="options">配置</param>
    public ReduceJobService(IQueue<TItem> queue, IQueueProcessor<TItem> processor, ReduceOptions options)
        : base(queue, processor)
    {
        _reduceTime = options.ReduceTime;
        _itemLife = options.ItemLife;
        _pool = new(queue, processor, (int)options.ConcurrencyLevel);
        if (options.AutoStart)
            Start();
    }
    #region 配置
    /// <summary>
    /// 休眠暂停时间
    /// </summary>
    protected readonly TimeSpan _reduceTime;
    /// <summary>
    /// 休眠暂停时间
    /// </summary>
    public TimeSpan ReduceTime 
        => _reduceTime;
    /// <summary>
    /// 服务池
    /// </summary>
    private readonly ThreadJobPool<TItem> _pool;
    /// <summary>
    /// 子元素寿命
    /// </summary>
    private readonly TimeSpan _itemLife;
    /// <summary>
    /// 服务池
    /// </summary>
    public ThreadJobPool<TItem> Pool
        => _pool;
    #endregion
    /// <inheritdoc />
    protected override async void Run(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _lastTime = DateTime.Now;
            CheckLife(_pool, _itemLife);
            CheckIncrement(_pool, _queue);
            try
            {
                await Task.Delay(_reduceTime, token)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }
    /// <inheritdoc />
    public override bool Stop()
    {
        base.Stop();
        _pool.Stop();
        return true;
    }
    /// <summary>
    /// 检查超期线程
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="itemLife"></param>
    private static void CheckLife(ThreadJobPool<TItem> pool, TimeSpan itemLife)
    {
        var list = pool.Actives;
        foreach (var item in list)
        {
            if (item.Started && item.LastTime.Add(itemLife) < DateTime.Now)
            {
                // 超过阈值配置ItemLife
                // 关闭回收超期线程
                item.Dispose();
                // 任务超时(失效),尝试取消
                if (item.LastItem is ICancelable cancelable)
                    Job.Processor.Cancel(cancelable);
            }
            else if(item.LastItem is IState<bool> state && !state.Status)
            {
                // 关闭回收超期线程
                item.Dispose();
                // 任务超时(失效),尝试取消
                if (state is ICancelable cancelable)
                    Job.Processor.Cancel(cancelable);
            }
        }
    }
    /// <summary>
    /// 尝试激活线程
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="queue"></param>
    private static void CheckIncrement(ThreadJobPool<TItem> pool, IQueue<TItem> queue)
    {
        if (queue.IsEmpty)
            return;
        // 非空队列,激活线程
        pool.Increment();
    }
}
