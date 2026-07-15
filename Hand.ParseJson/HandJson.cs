using Hand.Configuration;
using Hand.Convert;
using Hand.Creational;
using Hand.ParseJson.Cachers;
using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using Hand.Utf8;

namespace Hand.ParseJson;

/// <summary>
/// Json读取配置
/// </summary>
public class HandJson(IMemberBuilderProvider builderProvider, DefaultValueBuilder defaultValue)
{
    /// <summary>
    /// Json读取配置
    /// </summary>
    public HandJson()
        : this(MemberBuilderProvider.Instance, DefaultValueBuilder.Instance)
    {
    }
    #region 配置
    private readonly IMemberBuilderProvider _builders = builderProvider;
    private readonly DefaultValueBuilder _defaultValues = defaultValue;
    private readonly PrimitiveReaderCacher _primitives = new(defaultValue);

    /// <summary>
    /// 构造器提供者
    /// </summary>
    public IMemberBuilderProvider Builders
        => _builders;
    /// <summary>
    /// 基础类型缓存
    /// </summary>
    internal PrimitiveReaderCacher Primitives
        => _primitives;
    /// <summary>
    /// 默认值提供者
    /// </summary>
    public DefaultValueBuilder DefaultValues
        => _defaultValues;
    #endregion
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public Primitives.StringReader String()
        => new(_defaultValues.Get<string>());
    #region Bool
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public Primitives.BoolReader Bool()
        => new(_defaultValues.Get<bool>());
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public static Primitives.BoolReader<TValue> Bool<TValue>(TValue trueValue, TValue falseValue, TValue defaultValue)
        => new(trueValue, falseValue, defaultValue);
    #endregion
    /// <summary>
    /// 值读取器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public IJsonParser<TValue> Value<TValue>()
        => _primitives.Get<TValue>() ?? throw new ArgumentException($"不支持类型{typeof(TValue).FullName}");
    #region Property
    /// <summary>
    /// 属性读取
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="name"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public PropertyParser<TValue> Property<TValue>(string name, IJsonParser<TValue> original)
    {
        if(original is IDefault<TValue> @default)
            return new(name, original, @default.DefaultValue);
        return new(name, original, _defaultValues.Get<TValue>());
    }
    /// <summary>
    /// 属性读取
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public PropertyParser<TValue> Property<TValue>(string name)
        => new(name, Value<TValue>(), _defaultValues.Get<TValue>());
    /// <summary>
    /// 属性读取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PropertyParser<string> Property(string name)
        => new(name, String(), _defaultValues.Get<string>());
    #endregion
    #region PropertyName
    /// <summary>
    /// 属性名读取
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public PropertyNameParser<TValue> PropertyName<TValue>()
        => new(PrimitiveReaderCacher.Parser as ISpanParser<byte, TValue> ?? throw new NotSupportedException($"不支持类型{typeof(TValue).FullName}"), _defaultValues.Get<TValue>());
    /// <summary>
    /// 属性名读取
    /// </summary>
    /// <returns></returns>
    public PropertyNameParser<string> PropertyName()
        => new(StringConverter.Instance, _defaultValues.Get<string>());
    #endregion
    #region Entity
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>(ICreator<IMemberBuilder<TEntity>> creator)
        => new(this, creator, _defaultValues.Get<TEntity>());
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity, TBuilder>()
        where TBuilder : IMemberBuilder<TEntity>, new()
        => new(this, new DelegateCreator<IMemberBuilder<TEntity>>(() => new TBuilder()), _defaultValues.Get<TEntity>());
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>()
        => new(this, _builders.Get<TEntity>() ?? throw new NotSupportedException("请先配置构造器或传入构造器"), _defaultValues.Get<TEntity>());
    #endregion
    #region Dictionary
    /// <summary>
    /// 字典解析器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    public DictionaryParser<TKey, TValue> Dictionary<TKey, TValue>(IJsonParser<TValue> value, bool acceptDefault = false)
        where TKey : notnull
        => new(PropertyName<TKey>(), value, EqualityComparer<TKey>.Default, acceptDefault);
    /// <summary>
    /// 字典解析器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    public DictionaryParser<TKey, TValue> Dictionary<TKey, TValue>(bool acceptDefault = false)
        where TKey : notnull
        => new(PropertyName<TKey>(), Value<TValue>(), EqualityComparer<TKey>.Default, acceptDefault);
    /// <summary>
    /// 字典解析器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    public DictionaryParser<string, TValue> Dictionary<TValue>(IJsonParser<TValue> value, bool acceptDefault = false)
        => new(PropertyName(), value, StringComparer.Ordinal, acceptDefault);
    /// <summary>
    /// 字典解析器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    public DictionaryParser<string, TValue> Dictionary<TValue>(bool acceptDefault = false)
        => new(PropertyName(), Value<TValue>(), StringComparer.Ordinal, acceptDefault);
    /// <summary>
    /// 字典解析器
    /// </summary>
    /// <returns></returns>
    /// <param name="acceptDefault"></param>
    public DictionaryParser<string, object> Dictionary(bool acceptDefault = false)
        => new(PropertyName(), Value<object>(), StringComparer.Ordinal, acceptDefault);
    #endregion
    /// <summary>
    /// Object开始
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public ObjectParser<TResult> Object<TResult>(IJsonParser<TResult> parser)
    {
        if (parser is IDefault<TResult> @default)
            return new(parser, @default.DefaultValue);
        return new(parser, _defaultValues.Get<TResult>());
    }
    /// <summary>
    /// Array开始
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public ArrayParser<TResult> Array<TResult>(IJsonParser<TResult> parser)
    {
        if (parser is IDefault<TResult> @default)
            return new(parser, @default.DefaultValue);
        return new(parser, _defaultValues.Get<TResult>());
    }
    #region First
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(IJsonParser<TResult> parser)
    {
        if (parser is IDefault<TResult> @default)
            return new(parser, @default.DefaultValue);
        return new(parser, _defaultValues.Get<TResult>());
    }
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(string property)
    {
        var @default = _defaultValues.Get<TResult>();
        return new(new PropertyParser<TResult>(property, Value<TResult>(), @default), @default);
    }
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public FirstReader<string> First(string property)
    {
        var @default = _defaultValues.Get<string>();
        return new(new PropertyParser<string>(property, new Primitives.StringReader(@default), @default), @default);
    }
    #endregion

    /// <summary>
    /// 默认实例
    /// </summary>
    public static HandJson Default
        => Inner.Default;
    /// <summary>
    /// 内部缓存
    /// </summary>
    static class Inner
    {
        public static readonly HandJson Default = new();
    }
}
