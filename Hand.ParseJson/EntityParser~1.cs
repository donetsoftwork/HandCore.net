using Hand.Creational;
using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 实体解析器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="json"></param>
/// <param name="creator"></param>
/// <param name="defaultValue"></param>
public class EntityParser<TEntity>(HandJson json, ICreator<IMemberBuilder<TEntity>> creator, TEntity defaultValue)
    : ValueReader<TEntity>(defaultValue), IEntityParser<TEntity>
{
    /// <summary>
    /// 实体解析器
    /// </summary>
    /// <param name="json"></param>
    public EntityParser(HandJson json)
        : this(json, json.BuilderProvider.Get<TEntity>()!, json.DefaultValue.Get<TEntity>())
    {
    }

    #region 配置
    /// <summary>
    /// json读取配置
    /// </summary>
    protected readonly HandJson _json = json;
    private readonly ICreator<IMemberBuilder<TEntity>> _creator = creator;
    private readonly Dictionary<string, IMemberParser> _properties = [];
    
    /// <inheritdoc />
    public HandJson Json
        => _json;
    /// <summary>
    /// 属性
    /// </summary>
    public IEnumerable<IMemberParser> Properties
        => _properties.Values;
    #endregion

    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="property"></param>
    public void AddProperty(string name, IMemberParser property)
        => _properties[name] = property;
    /// <summary>
    /// 新构造器
    /// </summary>
    /// <returns></returns>
    public virtual IMemberBuilder<TEntity> New()
        => _creator.Create();

    /// <inheritdoc />
    protected TEntity GetValue(ref Utf8JsonReader reader)
    {
        var builder = New();
        var currentDepth = reader.CurrentDepth;
        do
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    ReadProperty(builder, ref reader);
                    break;
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    // 检查是否到达结束节点
                    if (reader.CurrentDepth <= currentDepth)
                        goto Build;
                    break;
                //case JsonTokenType.None:
                //    break;
                default:
                    //// 检查是否到达结束节点
                    //if (reader.CurrentDepth <= currentDepth)
                    //    goto Build;
                    break;
            }
        } while (reader.Read());
    Build:
        return builder.Build();
    }
    /// <inheritdoc />
    public override bool TryParser(ref Utf8JsonReader reader, out TEntity result)
    {
        result = GetValue(ref reader);
        return true;
    }
    /// <inheritdoc />
    protected override bool TryParser(ReadOnlySpan<byte> bytes, out TEntity result)
    {
        // 只实现基类,并不实际调用
        result = _defaultValue;
        return false;
    }
    /// <summary>
    /// 读取属性
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    public virtual void ReadProperty(IMemberStore entity, ref Utf8JsonReader reader)
    {
        var propertyName = reader.GetString();
        if(propertyName is null)
            return;
        ReadProperty(entity, ref reader, propertyName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    public virtual void ReadProperty(IMemberStore entity, ref Utf8JsonReader reader, string name)
    {
        if (_properties.TryGetValue(name, out var property))
            property.Save(entity, ref reader);
    }
    #region WithProperty
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="parser"></param>
    /// <param name="property"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithProperty<TProperty>(IJsonParser<TProperty> parser, string property, string member)
    {
        AddProperty(property, new MemberParser<TProperty>(member, parser));
        return this;
    }
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="parser"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithProperty<TProperty>(IJsonParser<TProperty> parser, string name)
        => WithProperty(parser, name, name);
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="property"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithProperty<TProperty>(string property, string member)
        => WithProperty(_json.Value<TProperty>(), property, member);
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public EntityParser<TEntity> WithProperty<TProperty>(string name)
        => WithProperty<TProperty>(name, name);
    #endregion
}
