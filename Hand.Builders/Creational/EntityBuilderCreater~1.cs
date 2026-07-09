using Hand.Builders;
using System.Reflection;

namespace Hand.Creational;

/// <summary>
/// 实体建造者工厂
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="constructor"></param>
/// <param name="parameterNames"></param>
/// <param name="properties"></param>
/// <param name="fields"></param>
public class EntityBuilderCreater<TEntity>(ConstructorInfo constructor, IDictionary<string, int> parameterNames, IDictionary<string, PropertyInfo> properties, IDictionary<string, FieldInfo> fields)
    : ICreator<IMemberBuilder<TEntity>>
{
    #region 配置
    private readonly ConstructorInfo _constructor = constructor;
    private readonly int parameterCount = parameterNames.Count;
    private readonly IDictionary<string, int> _parameterNames = parameterNames;
    private readonly IDictionary<string, PropertyInfo> _properties = properties;
    private readonly IDictionary<string, FieldInfo> _fields = fields;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor
        => _constructor;
    /// <summary>
    /// 参数名
    /// </summary>
    public IDictionary<string, int> ParameterNames
        => _parameterNames;
    /// <summary>
    /// 参数数量
    /// </summary>
    public int ParameterCount
        => parameterCount;
    /// <summary>
    /// 属性
    /// </summary>
    public IDictionary<string, PropertyInfo> Properties
        => _properties;
    /// <summary>
    /// 字段
    /// </summary>
    public IDictionary<string, FieldInfo> Fields
        => _fields;
    #endregion
    /// <inheritdoc />
    public IMemberBuilder<TEntity> Create()
    {
        var parameters = new object[parameterCount];
        return new EntityBuilder<TEntity>(_constructor, _parameterNames, parameters, _properties, _fields);
    }
}
