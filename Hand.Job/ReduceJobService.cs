namespace Hand.Job;

/// <summary>
/// 节约作业服务
/// </summary>
public class ReduceJobService
    : ThreadJobService
{
    /// <summary>
    /// 节约作业服务
    /// </summary>
    /// <param name="processor">操作</param>
    /// <param name="options">配置</param>
    public ReduceJobService(IProcessor processor, ReduceOptions options)
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
    protected override async void Run(IProcessor processor, CancellationToken token)
    {
        while (true)
        {
            if (!processor.Run())
                await Task.Delay(_reduceTime, CancellationToken.None).ConfigureAwait(false);
            if (token.IsCancellationRequested)
                break;
        }
    }
    /// <summary>
    /// 构造作业服务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ReduceJobService Create(IProcessor processor, ReduceOptions options)
    {
        var concurrencyLevel = options.ConcurrencyLevel;
        if (concurrencyLevel < 1u)
            return new ReduceJobService(processor, options);
        return new ConcurrentJobService(processor, options);
    }
}
