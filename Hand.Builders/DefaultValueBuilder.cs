using Hand.Cache;
using Hand.Cachers;

namespace Hand;

/// <summary>
/// 默认值构造器
/// </summary>
/// <param name="cacher"></param>
public sealed class DefaultValueBuilder(IGenericCacher<Type> cacher)
{
    /// <summary>
    /// 默认值构造器
    /// </summary>
    public DefaultValueBuilder()
        : this(new DictionaryCacher<Type>())
    {
    }
    private readonly DefaultValueCacher _cacher = new(cacher);

    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public TValue Get<TValue>()
        => _cacher.Get<TValue>(typeof(TValue));
    /// <summary>
    /// 设置默认值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public DefaultValueBuilder Use<TValue>(TValue value)
    {
        _cacher.Save(typeof(TValue), value);
        return this;
    }
    /// <summary>
    /// 默认实例
    /// </summary>
    public static DefaultValueBuilder Instance
        => Inner.Instance;
    /// <summary>
    /// 内部缓存
    /// </summary>
    static class Inner
    {
        public static readonly DefaultValueBuilder Instance = new();
    }
}
