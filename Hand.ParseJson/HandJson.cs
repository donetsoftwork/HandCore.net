using Hand.Configuration;
using Hand.Creational;
using Hand.ParseJson.Cachers;
using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using Hand.Storage;
using System.Xml;

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
    private readonly IMemberBuilderProvider _builderProvider = builderProvider;
    private readonly DefaultValueBuilder _defaultValue = defaultValue;
    private readonly PrimitiveReaderCacher _primitiveCacher = new(defaultValue);

    /// <summary>
    /// 构造器提供者
    /// </summary>
    public IMemberBuilderProvider BuilderProvider
        => _builderProvider;
    /// <summary>
    /// 默认值提供者
    /// </summary>
    public DefaultValueBuilder DefaultValue
        => _defaultValue;
    /// <summary>
    /// 基础类型缓存
    /// </summary>
    internal PrimitiveReaderCacher PrimitiveCacher
        => _primitiveCacher;
    #endregion
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public Primitives.StringReader String()
        => new(_defaultValue.Get<string>());
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public Primitives.BoolReader Bool()
        => new(_defaultValue.Get<bool>());
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public static Primitives.BoolReader<TValue> Bool<TValue>(TValue trueValue, TValue falseValue, TValue defaultValue)
        => new(trueValue, falseValue, defaultValue);
    /// <summary>
    /// 值读取器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public IJsonParser<TValue> Value<TValue>()
        => _primitiveCacher.Get<TValue>() ?? throw new ArgumentException($"不支持类型{typeof(TValue).FullName}");
    //#region WithProperty
    ///// <summary>
    ///// 添加节点
    ///// </summary>
    ///// <typeparam name="TEntityParser"></typeparam>
    ///// <typeparam name="TProperty"></typeparam>
    ///// <param name="entity"></param>
    ///// <param name="parser"></param>
    ///// <param name="property"></param>
    ///// <param name="member"></param>
    ///// <returns></returns>
    //public static TEntityParser WithProperty<TEntityParser, TProperty>(TEntityParser entity, IJsonParser<TProperty> parser, string property, string member)
    //    where TEntityParser : IEntityParser
    //{
    //    entity.AddProperty(property, new MemberParser<TProperty>(member, parser));
    //    return entity;
    //}
    ///// <summary>
    ///// 添加节点
    ///// </summary>
    ///// <typeparam name="TEntityParser"></typeparam>
    ///// <typeparam name="TProperty"></typeparam>
    ///// <param name="entity"></param>
    ///// <param name="property"></param>
    ///// <param name="member"></param>
    ///// <returns></returns>
    //public TEntityParser WithProperty<TEntityParser, TProperty>(TEntityParser entity, string property, string member)
    //    where TEntityParser : IEntityParser
    //    => WithProperty(entity, Value<TProperty>(), property, member);
    /////// <summary>
    /////// 添加节点
    /////// </summary>
    /////// <typeparam name="TEntityParser"></typeparam>
    /////// <typeparam name="TProperty"></typeparam>
    /////// <param name="entity"></param>
    /////// <param name="name"></param>
    /////// <returns></returns>
    ////public TEntityParser WithProperty<TEntityParser, TProperty>(TEntityParser entity, string name)
    ////    where TEntityParser : IEntityParser
    ////    => WithProperty(entity, Value<TProperty>(), name, name);
    //#endregion

    #region Entity
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>(ICreator<IMemberBuilder<TEntity>> creator)
        => new(this, creator, _defaultValue.Get<TEntity>());
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity, TBuilder>()
        where TBuilder : IMemberBuilder<TEntity>, new()
        => new(this, new DelegateCreator<IMemberBuilder<TEntity>>(() => new TBuilder()), _defaultValue.Get<TEntity>());
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>()
        => new(this, _builderProvider.Get<TEntity>() ?? throw new NotSupportedException("请先配置构造器或传入构造器"), _defaultValue.Get<TEntity>());
    #endregion
    #region First
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="property"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(string property, IJsonParser<TResult> parser)
    {
        if (parser is IDefault<TResult> @default)
            return new(property, parser, @default.DefaultValue);
        return new(property, parser, _defaultValue.Get<TResult>());
    }
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(IJsonParser<TResult> parser)
        => First(typeof(TResult).Name, parser);
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(string property)
        => First(property, Value<TResult>());
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public FirstReader<string> First(string property)
    {
        var @default = _defaultValue.Get<string>();
        return new(property, new Primitives.StringReader(@default), @default);
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
