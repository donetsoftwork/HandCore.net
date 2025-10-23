namespace Hand.Models;

/// <summary>
/// 领域实体
/// </summary>
/// <typeparam name="TEntityId"></typeparam>
public interface IDomainIEntity<out TEntityId>
    : IEntity<TEntityId>
{
    /// <summary>
    /// 实体状态
    /// </summary>
    EntityStatus Status { get; set; }
}
