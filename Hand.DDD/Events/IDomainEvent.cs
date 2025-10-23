namespace Hand.Events;

/// <summary>
/// 领域事件
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// 聚合根Id
    /// </summary>
    long RootId { get; }
}
