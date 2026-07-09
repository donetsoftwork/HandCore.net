namespace Hand.Creational;

/// <summary>
/// 成员建造者
/// </summary>
public interface IMemberBuilder<TEntity>
    : IMemberStore
{
    /// <summary>
    /// 构造
    /// </summary>
    /// <returns></returns>
    TEntity Build();
}
