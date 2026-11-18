using Hand.Collections;
using Hand.States;

namespace Hand.Job;

/// <summary>
/// 线程作业服务
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="queue"></param>
/// <param name="processor"></param>
public class ThreadJobService<TItem>(IQueue<TItem> queue, IQueueProcessor<TItem> processor)
    : IJobService, IActive<TItem>, IDisposable
{
    #region 配置
    /// <summary>
    /// 队列
    /// </summary>
    protected readonly IQueue<TItem> _queue = queue;
    /// <summary>
    /// 操作
    /// </summary>
    protected readonly IQueueProcessor<TItem> _processor = processor;
    /// <summary>
    /// Token
    /// </summary>
    private CancellationTokenSource? _source = null;
    /// <summary>
    /// 状态(1为正在启动,0为未启动,2为已启动,-1为正在停止)
    /// </summary>
    private int _state = 0;
    /// <summary>
    /// 最近活动时间
    /// </summary>
    protected DateTime _lastTime = DateTime.Now;
    private TItem? _lastItem;
    /// <summary>
    /// 是否已启动
    /// </summary>
    public bool Started
        => Volatile.Read(ref _state) == 2;
    /// <summary>
    /// 最近执行时间
    /// </summary>
    public DateTime LastTime 
        => _lastTime;
    /// <summary>
    /// 最近执行元素
    /// </summary>
    public TItem? LastItem
        => _lastItem;
    /// <summary>
    /// 队列
    /// </summary>
    public IQueue<TItem> Queue
        => _queue;
    /// <summary>
    /// 处理器
    /// </summary>
    public IQueueProcessor<TItem> Processor
        => _processor;
    #endregion
    #region IActive<TItem>
    /// <inheritdoc />
    public virtual bool Activate(TItem instance)
    {
        if (Volatile.Read(ref _state) >= 1)
        {
            if(_source == null || _source.IsCancellationRequested) 
                return false;
            _lastTime = DateTime.Now;
            _lastItem = instance;
            //Console.WriteLine($"Activate:{_state}");
            return true;
        }
        //Console.WriteLine($"Activate:{_state}");
        return false;
    }
    #endregion
    /// <inheritdoc />
    public virtual bool Start()
    {
        if (Interlocked‌.CompareExchange(ref _state, 1, 0) == 0)
        {
            _source = new();
            StartThread(_source.Token);
            return Interlocked‌.CompareExchange(ref _state, 2, 1) == 1;
        }
        return false;
    }
    /// <summary>
    /// 启动线程
    /// </summary>
    protected virtual void StartThread(CancellationToken token)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        ThreadPool.QueueUserWorkItem(Run, token, true);
#else
        ThreadPool.QueueUserWorkItem(state => Run((CancellationToken)state), token);
#endif
    }
    /// <inheritdoc />
    public virtual bool Stop()
    {
        if (Interlocked‌.CompareExchange(ref _state, -1, 2) == 2)
        {
            try
            {
                _source?.Cancel();
            }
            catch { }
            return Interlocked‌.CompareExchange(ref _state, 0, -1) == -1;
        }
        return false;
    }
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="token"></param>
    protected virtual void Run(CancellationToken token)
    {
        if (_queue is null || _processor is null)
            return;
        _processor.Run(_queue, this, token);
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="item"></param>
    public void Add(TItem item)
        => _queue.Enqueue(item);
    /// <summary>
    /// 回收资源(停止线程)
    /// </summary>
    public virtual void Dispose()
         => Stop();
}
