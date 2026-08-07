namespace Hand.Collections;

/// <summary>
/// 计数结果
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="total"></param>
/// <param name="items"></param>
public class CountedResult<T>(int total, T[] items)
{
    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; set; } = total;
    /// <summary>
    /// 数据
    /// </summary>
    public T[] Items { get; set; } = items;
}
