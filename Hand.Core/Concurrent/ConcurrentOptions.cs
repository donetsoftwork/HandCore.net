namespace Hand.Concurrent;

/// <summary>
/// 并发配置
/// </summary>
public class ConcurrentOptions
{
    /// <summary>
    /// 事件总线并发上限，默认ushort.MaxValue
    /// </summary>
    public uint ConcurrencyLevel { get; set; } = ushort.MaxValue;
}
