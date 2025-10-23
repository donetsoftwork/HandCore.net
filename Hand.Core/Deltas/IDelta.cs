namespace Hand.Deltas;

/// <summary>
/// 增量
/// </summary>
public interface IDelta
{
    /// <summary>
    /// 变化的数据
    /// </summary>
    IDictionary<string, object> Data { get; }
}
