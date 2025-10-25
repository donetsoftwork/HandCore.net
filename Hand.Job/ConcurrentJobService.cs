namespace Hand.Job;

/// <summary>
/// 并发作业服务
/// </summary>
public class ConcurrentJobService
    : ReduceJobService, IProcessor
{
    #region 配置
    private readonly IProcessor _processor;
    /// <summary>
    /// 服务池
    /// </summary>
    private readonly ThreadJobPool _pool;
    /// <summary>
    /// 服务池
    /// </summary>
    public ThreadJobPool Pool 
        => _pool;
    /// <summary>
    /// 作业执行对象
    /// </summary>
    public IProcessor Processor 
        => _processor;
    #endregion
    /// <summary>
    /// 并发作业服务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="options"></param>
    public ConcurrentJobService(IProcessor processor, ReduceOptions options)
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
    protected override async void Run(IProcessor processor, CancellationToken token)
    {
        while (true)
        {
            if (_processor.Run())
            {
                _pool.Increment();
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
    /// <inheritdoc />
    bool IProcessor.Run()
    {
        if(_processor.Run())
        {
            _pool.Increment();
            return true;
        }
        return false;
    }
}
