using Hand.Configuration;
using Hand.Convert;
using Hand.Creational;
using Hand.Maping;
using Hand.ParseXml.Cachers;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Move;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// Xml读取配置
/// </summary>
/// <param name="builders"></param>
/// <param name="defaultValues"></param>
public class HandXml(IMemberBuilderProvider builders, DefaultValueBuilder defaultValues)
{
    /// <summary>
    /// Xml读取配置
    /// </summary>
    public HandXml()
        : this(MemberBuilderProvider.Instance, DefaultValueBuilder.Instance)
    {
    }
    #region 配置
    private readonly IMemberBuilderProvider _builders = builders;
    private readonly DefaultValueBuilder _defaultValues = defaultValues;
    private readonly XmlConvertCacher _converters = new();

    /// <summary>
    /// 构造器提供者
    /// </summary>
    public IMemberBuilderProvider Builders
        => _builders;
    /// <summary>
    /// 转化器
    /// </summary>
    internal XmlConvertCacher Converters 
        => _converters;
    /// <summary>
    /// 默认值提供者
    /// </summary>
    public DefaultValueBuilder DefaultValues
        => _defaultValues;
    #endregion
    /// <summary>
    /// 基础类型读取器
    /// </summary>
    /// <typeparam name="TPrimitive"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public IParser<XmlReader, TPrimitive> Primitive<TPrimitive>(IParser<XmlReader, string> original)
    {
        if (original is IParser<XmlReader, TPrimitive> reader)
            return reader;
        return new PrimitiveReader<TPrimitive>(original, _converters.Get<TPrimitive>(), _defaultValues.Get<TPrimitive>());
    }
    #region Attribute
    /// <summary>
    /// 属性读取器
    /// </summary>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public AttributeReader Attribute(string attributeName)
        => new(attributeName, _defaultValues.Get<string>());
    /// <summary>
    /// 属性读取器
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public IParser<XmlReader, TAttribute> Attribute<TAttribute>(string attributeName)
        => Primitive<TAttribute>(new AttributeReader(attributeName, _defaultValues.Get<string>()));
    /// <summary>
    /// 属性读取器
    /// </summary>
    /// <param name="attributeIndex"></param>
    /// <returns></returns>
    public IndexAttributeReader Attribute(int attributeIndex)
        => new(attributeIndex, _defaultValues.Get<string>());
    /// <summary>
    /// 属性读取器
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="attributeIndex"></param>
    /// <returns></returns>
    public IParser<XmlReader, TAttribute> Attribute<TAttribute>(int attributeIndex)
        => Primitive<TAttribute>(new IndexAttributeReader(attributeIndex, _defaultValues.Get<string>()));
    #endregion
    #region Content
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <returns></returns>
    public ContentReader Content()
        => new(_defaultValues.Get<string>());
    /// <summary>
    /// 文本读取器
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    /// <returns></returns>
    public IParser<XmlReader, TContent> Content<TContent>()
        => Primitive<TContent>(new ContentReader(_defaultValues.Get<string>()));
    #endregion
    #region Name
    /// <summary>
    /// 属性名读取
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public IParser<XmlReader, TValue> Name<TValue>()
        => Primitive<TValue>(new ElementNameParser(_defaultValues.Get<string>()));
    /// <summary>
    /// 属性名读取
    /// </summary>
    /// <returns></returns>
    public ElementNameParser Name()
        => new(_defaultValues.Get<string>());
    #endregion
    #region Entity
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="creator"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>(ICreator<IMemberBuilder<TEntity>> creator, IMemberParser? content = null)
        => new(this, creator, content);
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="creator"></param>
    /// <param name="contentName"></param>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity>(ICreator<IMemberBuilder<TEntity>> creator, string contentName)
        => new(this, creator, Content().Member(contentName));
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity, TBuilder>(IMemberParser? content = null)
        where TBuilder : IMemberBuilder<TEntity>, new()
        => new(this, new DelegateCreator<IMemberBuilder<TEntity>>(() => new TBuilder()), content);
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="contentName"></param>
    /// <returns></returns>
    public EntityParser<TEntity> Entity<TEntity, TBuilder>(string contentName)
        where TBuilder : IMemberBuilder<TEntity>, new()
        => new(this, new DelegateCreator<IMemberBuilder<TEntity>>(() => new TBuilder()), Content().Member(contentName));
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public EntityParser<TEntity> Entity<TEntity>(IMemberParser? content = null)
        => new(this, _builders.Get<TEntity>() ?? throw new NotSupportedException("请先配置构造器或传入构造器"), content);
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="contentName"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public EntityParser<TEntity> Entity<TEntity>(string contentName)
        => new(this, _builders.Get<TEntity>() ?? throw new NotSupportedException("请先配置构造器或传入构造器"), Content().Member(contentName));
    #endregion
    #region Element
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="elementName"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    public ElementParser<TResult> Element<TResult>(string elementName, IParser<XmlReader, TResult> parser)
        => new(elementName, parser, _defaultValues.Get<TResult>());
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public ElementParser<TResult> Element<TResult>(IParser<XmlReader, TResult> parser)
        => new(typeof(TResult).Name, parser, _defaultValues.Get<TResult>());
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public ElementParser<string> Element(string element)
        => new(element, Content(), _defaultValues.Get<string>());
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <returns></returns>
    public ElementParser<TResult> Element<TResult>(string element)
        => new(element, Content<TResult>(), _defaultValues.Get<TResult>());
    #endregion
    /// <summary>
    /// 移入子节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public MoveInParser<TResult> MoveIn<TResult>(IParser<XmlReader, TResult> parser)
        => new(parser, _defaultValues.Get<TResult>());
    /// <summary>
    /// 移到子节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="name"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    public MoveToParser<TResult> MoveTo<TResult>(string name, IParser<XmlReader, TResult> parser)
        => new(name, parser, _defaultValues.Get<TResult>());
    #region First
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public FirstReader<TResult> First<TResult>(IParser<XmlReader, TResult> parser)
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
        return new(new ElementParser<TResult>(property, Content<TResult>(), @default), @default);
    }
    /// <summary>
    /// 获取单个节点
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public FirstReader<string> First(string property)
    {
        var @default = _defaultValues.Get<string>();
        return new(new ElementParser<string>(property, new ContentReader(@default), @default), @default);
    }
    #endregion
    #region Convert
    /// <summary>
    /// 配置转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="converter"></param>
    public HandXml Use<TValue, TConverter>(TConverter converter)
        where TConverter : IConverter<string, TValue>
    {
        _converters.Save(typeof(TValue), converter);
        return this;
    }
    /// <summary>
    /// 配置转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="converter"></param>
    public HandXml Use<TValue>(Converter<string, TValue> converter)
    {
        _converters.Save(typeof(TValue), new DelegateConverter<string, TValue>(converter));
        return this;
    }
    #endregion

    /// <summary>
    /// 默认实例
    /// </summary>
    public static HandXml Default
        => Inner.Default;
    /// <summary>
    /// 内部缓存
    /// </summary>
    static class Inner
    {
        public static readonly HandXml Default = new();
    }
}
