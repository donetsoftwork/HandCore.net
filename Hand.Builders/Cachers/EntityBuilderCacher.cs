using Hand.Cache;
using Hand.Creational;
using System.Reflection;

namespace Hand.Cachers;

/// <summary>
/// 实体建造工厂缓存
/// </summary>
/// <param name="cacher"></param>
public class EntityBuilderCacher(IGenericCacher<Type> cacher)
    : CacheFactoryBase<Type>(cacher)
{
    /// <inheritdoc />
    protected override TCreator CreateNew<TCreator>(in Type key)
    {
        // 反射调用泛型方法Create
        if (_createMethod.MakeGenericMethod(key).Invoke(null, []) is TCreator creator)
            return creator;
        return default!;
    }
    /// <inheritdoc />
    public void Add<TEntity>(ICreator<IMemberBuilder<TEntity>> creator)
        => Save(typeof(TEntity), creator);
    /// <summary>
    /// 获取成员建造者工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public ICreator<IMemberBuilder<TEntity>>? Get<TEntity>()
        => Get<ICreator<IMemberBuilder<TEntity>>?>(typeof(TEntity));

    /// <summary>
    /// 构造
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static ICreator<IMemberBuilder<TEntity>>? Create<TEntity>()
    {
        var entityType = typeof(TEntity);
        var info = InitializerProvider.Desc.Get(entityType);
        if (info is null)
            return null;
        var constructor = info.Constructor;
        var parameterNames = CheckParameterNames(constructor.GetParameters());
        return new EntityBuilderCreater<TEntity>(constructor, parameterNames, info.Properties, info.Fields);
    }
    /// <summary>
    /// 构造参数名
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IDictionary<string, int> CheckParameterNames(ParameterInfo[] parameters)
    {
        var count = parameters.Length;
        var names = new Dictionary<string, int>(count, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < count; i++)
            names[parameters[i].Name!] = i;
        return names;
    }
    /// <summary>
    /// 反射泛型方法Create
    /// </summary>
    private static readonly MethodInfo _createMethod = typeof(EntityBuilderCacher)
        .GetMethod(nameof(Create))!
        .GetGenericMethodDefinition();
}