using Hand.Creational;
using Hand.Maping;
using Hand.ParseXml.Contracts;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 实体解析器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="xml"></param>
/// <param name="creator"></param>
/// <param name="content"></param>
/// <param name="hasItem"></param>
public class EntityParser<TEntity>(HandXml xml, ICreator<IMemberBuilder<TEntity>> creator, IMemberParser? content, bool hasItem = false)
    : IEntityParser<TEntity>, IDataGet<XmlReader, TEntity>
{
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="content"></param>
    /// <param name="hasItem"></param>
    public EntityParser(HandXml xml, IMemberParser? content, bool hasItem = false)
        : this(xml, xml.Builders.Get<TEntity>()!, content, hasItem)
    {
    }
    #region 配置
    /// <summary>
    /// xml读取配置
    /// </summary>
    protected readonly HandXml _xml = xml;
    private readonly ICreator<IMemberBuilder<TEntity>> _creator = creator;
    private readonly List<IMemberParser> _attributes = [];
    private bool _hasItem = hasItem;
    private readonly Dictionary<string, IMemberParser> _items = [];
    private readonly IMemberParser? _content = content;

    /// <inheritdoc />
    public HandXml Xml
        => _xml;
    /// <summary>
    /// 属性
    /// </summary>
    public IEnumerable<IMemberParser> Attributes 
        => _attributes;
    /// <summary>
    /// 子节点
    /// </summary>
    public IDictionary<string, IMemberParser> Items
        => _items;
    /// <summary>
    /// 文本节点
    /// </summary>
    public IMemberParser? Content 
        => _content;
    /// <summary>
    /// 是否有子节点
    /// </summary>
    public bool HasItem
        => _hasItem;
    #endregion

    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="attribute"></param>
    public void AddAttribute(IMemberParser attribute)
    {
        _attributes.Add(attribute);
    }
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="name"></param>
    /// <param name="child"></param>
    public void AddItem(string name, IMemberParser child)
    {
        _items[name] = child;
        _hasItem = true;
    }
    /// <summary>
    /// 新构造器
    /// </summary>
    /// <returns></returns>
    public virtual IMemberBuilder<TEntity> New()
        => _creator.Create();

    /// <inheritdoc />
    public virtual TEntity Get(XmlReader reader)
    {
        var builder = New();
        var depth = reader.Depth;
        ReadAttributes(builder, reader);
        if (_hasItem)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        ReadItem(builder, reader, reader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        // 检查是否到达结束节点
                        if (reader.Depth <= depth)
                            goto Build;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            _content?.Save(builder, reader);
        }

    Build:
        return builder.Build();
    }
    /// <inheritdoc />
    public bool TryParser(XmlReader reader, out TEntity result)
    {
        result = Get(reader);
        return true;
    }
    /// <summary>
    /// 读取子节点
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual void ReadItem(IMemberBuilder<TEntity> entity, XmlReader reader, string name)
    {
        if (_items.TryGetValue(name, out var child))
            child.Save(entity, reader);
    }
    /// <summary>
    /// 读取属性
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    public virtual void ReadAttributes(IMemberStore entity, XmlReader reader)
    {
        foreach (var attribute in _attributes)
            attribute.Save(entity, reader);
    }
    #region WithAttribute
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithAttribute(string name)
        => WithAttribute(name, name);
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithAttribute(string attribute, string member)
    {
        AddAttribute(_xml.Attribute(attribute).Member(member));
        return this;
    }
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithAttribute<TAttribute>(string name)
        => WithAttribute<TAttribute>(name, name);
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="attribute"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithAttribute<TAttribute>(string attribute, string member)
    {
        AddAttribute(_xml.Attribute<TAttribute>(attribute).Member(member));
        return this;
    }
    #endregion
    #region WithItem
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem(string name)
        => WithItem(_xml.Content(), name, name);
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem(string node, string member)
        => WithItem(_xml.Content(), node, member);
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem<TItem>(string name)
        => WithItem(_xml.Content<TItem>(), name, name);
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="node"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem<TItem>(string node, string member)
        => WithItem(_xml.Content<TItem>(), node, member);
    /// <summary>
    /// 添加子节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="item"></param>
    /// <param name="node"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem<TItem>(IXmlParser<TItem> item, string node, string member)
    {
        AddItem(node, item.Member(member));
        return this;
    }
    /// <summary>
    /// 添加子节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="item"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithItem<TItem>(IXmlParser<TItem> item, string name)
        => WithItem(item, name, name);
    #endregion
    #region WithRepeat
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TRepeat"></typeparam>
    /// <param name="repeat"></param>
    /// <param name="converter"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem, TRepeat>(RepeatReader<TItem> repeat, IConverter<IEnumerable<TItem>, TRepeat> converter, string member)
        => WithItem(repeat.Convert(converter), repeat.Name, member);
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TRepeat"></typeparam>
    /// <param name="repeat"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem, TRepeat>(RepeatReader<TItem> repeat, IConverter<IEnumerable<TItem>, TRepeat> converter)
        => WithItem(repeat.Convert(converter), repeat.Name, repeat.Name);
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TRepeat"></typeparam>
    /// <param name="repeat"></param>
    /// <param name="convert"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem, TRepeat>(RepeatReader<TItem> repeat, Converter<IEnumerable<TItem>, TRepeat> convert, string member)
        => WithItem(repeat.Convert(convert), repeat.Name, member);
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TRepeat"></typeparam>
    /// <param name="repeat"></param>
    /// <param name="convert"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem, TRepeat>(RepeatReader<TItem> repeat, Converter<IEnumerable<TItem>, TRepeat> convert)
        => WithItem(repeat.Convert(convert), repeat.Name, repeat.Name);
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="repeat"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem>(RepeatReader<TItem> repeat, string member)
        => WithItem(repeat.ToArray(), repeat.Name, member);
    /// <summary>
    /// 添加重复节点解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="repeat"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithRepeat<TItem>(RepeatReader<TItem> repeat)
        => WithItem(repeat.ToArray(), repeat.Name, repeat.Name);
    #endregion
}
