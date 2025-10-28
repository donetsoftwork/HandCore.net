namespace Hand.Job;

/// <summary>
/// 线程作业服务
/// </summary>
/// <param name="processor"></param>
public class ThreadJobService<TItem>(IQueueProcessor<TItem> processor)
    : IJobService, IDisposable
{
    #region 配置
    /// <summary>
    /// 操作
    /// </summary>
    private readonly IQueueProcessor<TItem> _processor = processor;
    /// <summary>
    /// Token
    /// </summary>
    private CancellationTokenSource _source = null;
    private bool _started = false;
    /// <summary>
    /// 是否已启动
    /// </summary>
    public bool Started
        => _started;
    #endregion
    /// <inheritdoc />
    public virtual bool Start()
    {
        // 避免重复启动
        if (_started)
            return true;
        _started = true;
        _source = new();
        StartThread();
        return true;
    }
    /// <summary>
    /// 启动线程
    /// </summary>
    protected virtual void StartThread()
    {
        Thread thread = new(Run)
        {
            IsBackground = true
        };
        thread.Start();
        //ThreadPool.QueueUserWorkItem(_ => Run(), null);
    }
    /// <inheritdoc />
    public virtual bool Stop()
    {
        if(_started)
        {
            _source?.Cancel();
            _started = false;
        }
        return true;
    }
    /// <inheritdoc />
    public void Run()
    {
        var source = Volatile.Read(ref _source);
        if (source is null || _processor is null)
            return;
        Run(_processor, source.Token);
    }
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="token"></param>
    protected virtual void Run(IQueueProcessor<TItem> processor, CancellationToken token)
    {
        while (processor.TryTake(out var item))
        {
            processor.Run(ref item);
            if (token.IsCancellationRequested)
                break;
        }
    }
    /// <inheritdoc />
    void IDisposable.Dispose()
        => Dispose();
    /// <summary>
    /// 回收资源(停止线程)
    /// </summary>
    protected virtual void Dispose()
         => Stop();
}
