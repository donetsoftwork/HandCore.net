using Hand.Cache;
using Hand.Cachers;
using Hand.Creational;

namespace Hand;

/// <summary>
/// 实体建造提供者
/// </summary>
/// <param name="cacher"></param>
public class MemberBuilderProvider(IGenericCacher<Type> cacher)
    : IMemberBuilderProvider
{
    #region 配置
    private readonly IGenericCacher<Type> _cacher = cacher;
    #endregion
    /// <summary>
    /// 实体建造提供者
    /// </summary>
    public MemberBuilderProvider()
        : this(new EntityBuilderCacher(new DictionaryCacher<Type>()))
    {
    }
    /// <inheritdoc />
    public void Add<TEntity>(ICreator<IMemberBuilder<TEntity>> creator)
        => _cacher.Save(typeof(TEntity), creator);
    /// <summary>
    /// 获取成员建造者工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public ICreator<IMemberBuilder<TEntity>>? Get<TEntity>()
        => _cacher.Get<ICreator<IMemberBuilder<TEntity>>?>(typeof(TEntity));
    /// <summary>
    /// 默认实例
    /// </summary>
    public static MemberBuilderProvider Instance
        => Inner.Instance;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    static class Inner
    {
        public static readonly MemberBuilderProvider Instance = new();
    }
}