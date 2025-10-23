namespace Hand.Deltas;

/// <summary>
/// 增量
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IDelta<TInstance> : IDelta
{
    /// <summary>
    /// 实体
    /// </summary>
    TInstance Instance { get; }
}
