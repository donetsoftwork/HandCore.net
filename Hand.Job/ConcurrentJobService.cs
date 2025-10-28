namespace Hand.Job;

/// <summary>
/// 并发作业服务
/// </summary>
public class ConcurrentJobService<TItem>
    : ReduceJobService<TItem>, IQueueProcessor<TItem>
{
    #region 配置
    private readonly IQueueProcessor<TItem> _processor;
    /// <summary>
    /// 服务池
    /// </summary>
    private readonly ThreadJobPool<TItem> _pool;
    /// <summary>
    /// 服务池
    /// </summary>
    public ThreadJobPool<TItem> Pool 
        => _pool;
    /// <summary>
    /// 作业执行对象
    /// </summary>
    public IQueueProcessor<TItem> Processor 
        => _processor;
    #endregion
    /// <summary>
    /// 并发作业服务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="options"></param>
    public ConcurrentJobService(IQueueProcessor<TItem> processor, ReduceOptions options)
        : base(processor, options)
    {
        _processor = processor;
        _pool = new(this, (int)options.ConcurrencyLevel - 1);
    }
    /// <inheritdoc />
    public override bool Stop()
    {
        base.Stop();
        _pool.Stop();
        return true;
    }
    /// <inheritdoc />
    protected override async void Run(IQueueProcessor<TItem> processor, CancellationToken token)
    {
        while (true)
        {
            if (processor.TryTake(out var item))
            {
                _pool.Increment();
                processor.Run(ref item);
            }
            else
            {
                await Task.Delay(_reduceTime, CancellationToken.None)
                    .ConfigureAwait(false);
            }
            if (token.IsCancellationRequested)
                break;
        }
    }
    #region ICollectionProcessor<TItem>
    /// <inheritdoc />
    bool IQueueProcessor<TItem>.TryTake(out TItem item)
    {
        if( _processor.TryTake(out item))
        {
            _pool.Increment();
            return true;
        }
        return false;
    }
    /// <inheritdoc />
    bool IProcessor<TItem>.Run(ref TItem instance)
        => _processor.Run(ref instance);
    #endregion
}
