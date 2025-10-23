namespace Hand.Job;

/// <summary>
/// 节约作业服务
/// </summary>
/// <param name="processor">操作</param>
/// <param name="reduceTime">休眠暂停时间</param>
public class ReduceJobService(IProcessor processor, TimeSpan reduceTime)
    : ThreadJobService(processor)
{
    #region 配置
    /// <summary>
    /// 休眠暂停时间
    /// </summary>
    protected readonly TimeSpan _reduceTime = reduceTime;
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
}
