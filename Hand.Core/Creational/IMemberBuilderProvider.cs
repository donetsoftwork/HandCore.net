namespace Hand.Creational;

/// <summary>
/// 成员建造提供者
/// </summary>
public interface IMemberBuilderProvider
{
    /// <summary>
    /// 获取成员建造者工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    ICreator<IMemberBuilder<TEntity>>? Get<TEntity>();
    /// <summary>
    /// 添加建造者工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="creator"></param>
    void Add<TEntity>(ICreator<IMemberBuilder<TEntity>> creator);
}
