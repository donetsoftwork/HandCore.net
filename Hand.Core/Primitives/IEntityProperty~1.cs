using Hand.Structural;

namespace Hand.Primitives;

/// <summary>
/// 实体属性
/// </summary>
public interface IEntityProperty<TProperty> : IWrapper<TProperty>
    where TProperty : notnull;
