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
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var properties = declareType.GetTypeInfo().DeclaredProperties;
#else
        var properties = declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
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
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetProperties(declareType.GetTypeInfo());
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredProperties;
#else
        => declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
    #endregion
    #region GetFields
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields<TStructuralType>()
        => GetFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetFields(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredFields.Where(field => field.IsPublic && !field.IsStatic);
#else
        => declareType.GetFields(BindingFlags.Instance | BindingFlags.Public);
#endif
    #endregion
    #region GetStaticFields
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields<TStructuralType>()
        => GetStaticFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetStaticFields(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredFields.Where(field => field.IsPublic && field.IsStatic);
#else
        => declareType.GetFields(BindingFlags.Static | BindingFlags.Public);
#endif
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
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var constructors = declareType.GetTypeInfo().DeclaredConstructors;
#else
        var constructors = declareType.GetConstructors();
#endif
        foreach (var constructor in constructors)
        {
            if (filter(constructor.GetParameters()))
                return constructor;
        }
        return null;
    }
    /// <summary>
    /// 获取所有构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            => GetConstructors(declareType.GetTypeInfo());
        /// <summary>
        /// 获取所有构造函数
        /// </summary>
        /// <param name="declaringTypeInfo"></param>
        /// <returns></returns>
        public static IEnumerable<ConstructorInfo> GetConstructors(TypeInfo declaringTypeInfo)
            => declaringTypeInfo.DeclaredConstructors;
#else
        => declareType.GetConstructors();
#endif
    #endregion
    #region MethodInfo
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static MethodInfo GetMethod(Type declareType, string name)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => declareType.GetTypeInfo().GetDeclaredMethod(name);
#else
        => declareType.GetMethod(name);
#endif
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="name"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static MethodInfo GetMethod(Type declareType, string name, Type[] types)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    {
        return declareType.GetTypeInfo()
            .GetDeclaredMethods(name)
            .FirstOrDefault(method => MatchParameter(method.GetParameters(), types));
    }
#else
        => declareType.GetMethod(name, types);
#endif
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetMethods(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredMethods;
#else
        => declareType.GetMethods();
#endif
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
