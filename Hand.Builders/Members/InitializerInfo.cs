using System.Reflection;

namespace Hand.Members;

/// <summary>
/// 初始化信息
/// </summary>
/// <param name="constructor"></param>
/// <param name="parameters"></param>
/// <param name="properties"></param>
/// <param name="fields"></param>
public class InitializerInfo(ConstructorInfo constructor, IDictionary<string, ParameterInfo> parameters, IDictionary<string, PropertyInfo> properties, IDictionary<string, FieldInfo> fields)
{
    #region 配置
    private readonly ConstructorInfo _constructor = constructor;
    private readonly IDictionary<string, ParameterInfo> _parameters = parameters;
    private readonly IDictionary<string, PropertyInfo> _properties = properties;
    private readonly IDictionary<string, FieldInfo> _fields = fields;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor 
        => _constructor;
    /// <summary>
    /// 参数
    /// </summary>
    public IDictionary<string, ParameterInfo> Parameters 
        => _parameters;
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
}
