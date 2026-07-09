using Hand.Cache;

namespace Hand.Cachers;

/// <summary>
/// 默认值构造器
/// </summary>
/// <param name="cacher"></param>
internal sealed class DefaultValueCacher(IGenericCacher<Type> cacher)
    : CacheFactoryBase<Type>(cacher)
{
    /// <inheritdoc />
    protected override TValue CreateNew<TValue>(in Type key)
        => default!;
}
