using Hand.Concurrent;

namespace Hand.Job;

/// <summary>
/// 节约作业服务配置
/// </summary>
public class ReduceOptions
    : ConcurrentOptions
{
    /// <summary>
    /// 主线程休眠暂停时间，默认50毫秒
    /// </summary>
    public TimeSpan ReduceTime = TimeSpan.FromMilliseconds(50);
    /// <summary>
    /// 是否自动启动
    /// </summary>
    public bool AutoStart = true;
}
