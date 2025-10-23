namespace Hand.Models;

/// <summary>
/// 实体属性
/// </summary>
public interface IEntityProperty<TProperty>
{
    /// <summary>
    /// 属性实际值
    /// </summary>
    TProperty Value { get; }
}
