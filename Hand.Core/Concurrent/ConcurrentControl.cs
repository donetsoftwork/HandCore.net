namespace Hand.Concurrent;

/// <summary>
/// 并发控制器
/// </summary>
/// <param name="limit">并发上限</param>
/// <param name="count">并发数量</param>
public class ConcurrentControl(uint limit = ushort.MaxValue, uint count = 0)
{
    #region 配置
    private readonly int _limit = (int)limit;
    private volatile int _count = (int)count;
    /// <summary>
    /// 并发上限
    /// </summary>
    public int Limit
        => _limit;
    /// <summary>
    /// 并发数量
    /// </summary>
    public int Count
        => _count;
    #endregion
    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Increment(ushort value  = 1)
    {
        int val = value;
        var count = Interlocked.Add(ref _count, val);
        if (count > _limit)
        {
            // 补偿
            Interlocked.Add(ref _count, -val);
            return false;
        }
        return true;
    }
    /// <summary>
    /// 减少
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Decrement(ushort value = 1)
    {
        int val = value;
        var count = Interlocked.Add(ref _count, -val);
        if (count < 0)
        {
            // 补偿
            Interlocked.Add(ref _count, val);
            return false;
        }
        return true;
    }
}
