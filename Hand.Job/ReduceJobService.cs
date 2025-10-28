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
    /// <param name="processor">操作</param>
    /// <param name="options">配置</param>
    public ReduceJobService(IQueueProcessor<TItem> processor, ReduceOptions options)
        : base(processor)
    {
        _reduceTime = options.ReduceTime;
        if(options.AutoStart)
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
    #endregion
    /// <inheritdoc />
    protected override async void Run(IQueueProcessor<TItem> processor, CancellationToken token)
    {
        while (true)
        {
            if (processor.TryTake(out var item))
                processor.Run(ref item);
            await Task.Delay(_reduceTime, CancellationToken.None).ConfigureAwait(false);
            if (token.IsCancellationRequested)
                break;
        }
    }
}
