using System.Reflection;

namespace Hand.Reflection;

/// <summary>
/// 反射成员
/// </summary>
public static class ReflectionMember
{
    #region GetPropery
    /// <summary>
    /// 筛选属性
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static PropertyInfo GetPropery(Type declareType, Func<PropertyInfo, bool> filter)
    {
        var properties = declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propery in properties)
        {
            if (filter(propery))
                return propery;
        }
        return null;
    }
    #endregion
    #region Properties
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static PropertyInfo[] GetProperties<TStructuralType>()
        => GetProperties(typeof(TStructuralType));
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <returns></returns>
    public static PropertyInfo[] GetProperties(Type declareType)
        => declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    #endregion
    #region GetFields
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static FieldInfo[] GetFields<TStructuralType>()
        => GetFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <returns></returns>
    public static FieldInfo[] GetFields(Type declareType)
        => declareType.GetFields(BindingFlags.Instance | BindingFlags.Public);
    #endregion
    #region GetStaticFields
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static FieldInfo[] GetStaticFields<TStructuralType>()
        => GetStaticFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <returns></returns>
    public static FieldInfo[] GetStaticFields(Type declareType)
        => declareType.GetFields(BindingFlags.Static | BindingFlags.Public);
    #endregion
    #region ConstructorInfo
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="parameterType"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructorByParameterType(Type declareType, Type parameterType)
        => GetConstructor(
            declareType,
            parameters => parameters.Length == 1
                && PairTypeKey.CheckValueType(parameters[0].ParameterType, parameterType));
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructor(Type declareType, Func<ParameterInfo[], bool> filter)
    {
        var constructors = declareType.GetConstructors();
        foreach (var constructor in constructors)
        {
            if (filter(constructor.GetParameters()))
                return constructor;
        }
        return null;
    }
    #endregion
    #region MatchParameter
    /// <summary>
    /// 匹配参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static bool MatchParameter(ParameterInfo[] parameters, Type[] types)
    {
        var count = parameters.Length;
        if (count == types.Length)
        {
            for (int i = 0; i < count; i++)
            {
                if (parameters[i].ParameterType == types[i])
                    continue;
                else
                    return false;
            }
            return true;
        }
        return false;
    }
    #endregion
}
