using Hand.Creational;

namespace Hand;

/// <summary>
/// 建造扩展方法
/// </summary>
public static class HandBuildServices
{
    /// <summary>
    /// 注册工厂
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="provider"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    public static TProvider UseCreator<TProvider, TEntity>(this TProvider provider, ICreator<IMemberBuilder<TEntity>> creator)
        where TProvider : IMemberBuilderProvider
    {
        provider.Add(creator);
        return provider;
    }
    /// <summary>
    /// 注册工厂
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="provider"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TProvider UseCreator<TProvider, TEntity>(this TProvider provider, Func<IMemberBuilder<TEntity>> func)
        where TProvider : IMemberBuilderProvider
    {
        provider.Add(new DelegateCreator<IMemberBuilder<TEntity>>(func));
        return provider;
    }
}
