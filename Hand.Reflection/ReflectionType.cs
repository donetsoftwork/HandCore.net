using System.Reflection;

namespace Hand.Reflection;

/// <summary>
/// 反射类型
/// </summary>
public static class ReflectionType
{
    #region IsNullable
    /// <summary>
    /// 是否可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullable(Type type)
        => IsGenericType(type, typeof(Nullable<>));
    #endregion
    #region IsGenericType
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType">泛型</param>
    /// <returns></returns>
    public static bool IsGenericType(Type type, Type genericType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => IsGenericType(type.GetTypeInfo(), genericType);
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool IsGenericType(TypeInfo typeInfo, Type genericType)
        => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == genericType;
#else
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
#endif
    #endregion
    #region HasGenericType
    /// <summary>
    /// 判断是否包含泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool HasGenericType(Type type, Type genericType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => HasGenericType(type.GetTypeInfo(), genericType);
    /// <summary>
    /// 判断是否包含泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool HasGenericType(TypeInfo type, Type genericType)
#endif
    {
        if (IsGenericType(type, genericType))
            return true;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var interfaces = type.ImplementedInterfaces;
#else
        var interfaces = type.GetInterfaces();
#endif
        foreach (var subType in interfaces)
        {
            if (IsGenericType(subType, genericType))
                return true;
        }
        return false;
    }
    #endregion
    #region GetGenericCloseInterfaces
    /// <summary>
    /// 获取泛型闭合接口
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericInterface"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetGenericCloseInterfaces(Type type, Type genericInterface)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => IsGenericType(type, genericInterface) ? [type] : GetGenericCloseInterfaces(type.GetTypeInfo(), genericInterface);
    /// <summary>
    /// 获取泛型闭合接口
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericInterface"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetGenericCloseInterfaces(TypeInfo type, Type genericInterface)
    {
        var interfaces = type.ImplementedInterfaces;
#else
    {
        if (IsGenericType(type, genericInterface))
        {
            yield return type;
            yield break;
        }
        var interfaces = type.GetInterfaces();
#endif
        foreach (var item in interfaces)
        {
            if (IsGenericType(item, genericInterface))
                yield return item;
        }
    }
    #endregion
    /// <summary>
    /// 扫描泛型实现
    /// </summary>
    /// <param name="assemblies"></param>
    /// <param name="action"></param>
    /// <param name="interfaces"></param>
    public static void ScanInterfaceImp(Assembly[] assemblies, Action<Type, Type> action, params IEnumerable<Type> interfaces)
    {
        if (assemblies == null || assemblies.Length == 0)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            return;
#else
        {
            ScanInterfaceImp(Assembly.GetExecutingAssembly(), action, interfaces);
            return;
        }
#endif
        var types = assemblies.SelectMany(assembly => assembly
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            .DefinedTypes
#else
            .GetTypes()
#endif
            .Where(type => !type.IsInterface && !type.IsAbstract));
        ScanInterfaceImp(interfaces, types, action);
    }
    /// <summary>
    /// 扫描泛型实现
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="action"></param>
    /// <param name="interfaces"></param>
    public static void ScanInterfaceImp(Assembly assembly, Action<Type, Type> action, IEnumerable<Type> interfaces)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var types = assembly.DefinedTypes;
#else
        var types = assembly.GetTypes();
#endif
        ScanInterfaceImp(interfaces, types.Where(type => !type.IsInterface && !type.IsAbstract), action);
    }

    /// <summary>
    /// 扫描接口实现
    /// </summary>
    /// <param name="interfaces"></param>
    /// <param name="types"></param>
    /// <param name="action"></param>
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    public static void ScanInterfaceImp(IEnumerable<Type> interfaces, IEnumerable<TypeInfo> types, Action<Type, Type> action)
#else
    public static void ScanInterfaceImp(IEnumerable<Type> interfaces, IEnumerable<Type> types, Action<Type, Type> action)
#endif
    {
        foreach (var @interface in interfaces)
        {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            var typeInfo = @interface.GetTypeInfo();
            var isGeneric = typeInfo.IsGenericTypeDefinition;
#else
            var isGeneric = @interface.IsGenericTypeDefinition;
#endif
            if (isGeneric)
            {
                ScanGenericInterfaceImp(@interface, types, action);
            }
            else
            {
                foreach (var type in types)
                {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
                    var isImpl = PairTypeKey.CheckValueType(type, typeInfo);
#else
                    var isImpl = PairTypeKey.CheckValueType(type, @interface);
#endif
                    if (isImpl)
                        ScanImpCallBack(type, @interface, action);
                }
            }
        }
    }
    /// <summary>
    /// 扫描泛型接口实现
    /// </summary>
    /// <param name="genericInterface"></param>
    /// <param name="types"></param>
    /// <param name="action"></param>
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    public static void ScanGenericInterfaceImp(Type genericInterface, IEnumerable<TypeInfo> types, Action<Type, Type> action)
    {
        var interfaceParamCount = genericInterface.GetTypeInfo().GenericTypeParameters.Length;
#else
    public static void ScanGenericInterfaceImp(Type genericInterface, IEnumerable<Type> types, Action<Type, Type> action)
    {
        var interfaceParamCount = genericInterface.GetGenericArguments().Length;
#endif

        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition)
            {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
                var typeParamCount = type.GenericTypeParameters.Length;
#else
                var typeParamCount = type.GetGenericArguments().Length;
#endif
                // 泛型实现
                if (typeParamCount == interfaceParamCount
                    && IsGenericType(type, genericInterface))
                {
                    ScanImpCallBack(type, genericInterface, action);
                }
            }
            else
            {
                // 泛型接口闭合实现
                foreach (var @interface in GetGenericCloseInterfaces(type, genericInterface))
                {
                    ScanImpCallBack(type, @interface, action);
                }
            }
        }
    }

#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    private static void ScanImpCallBack(TypeInfo type, Type @interface, Action<Type, Type> action)
        => action(@interface, type.AsType());
#else
    private static void ScanImpCallBack(Type type, Type @interface, Action<Type, Type> action)
        => action(@interface, type);
#endif
    /// <summary>
    /// 获取子元素类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();
        var arguments = GetGenericArguments(type);
        var count = arguments.Length;
        if (count == 0)
            return null;
        if (HasGenericType(type, typeof(IDictionary<,>)))
        {
            if (count == 2)
                return arguments[1];
        }
        else if (HasGenericType(type, typeof(IEnumerable<>)))
        {
            if (count == 1)
                return arguments[0];
        }
        return null;
    }
    /// <summary>
    /// 获取泛型参数
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type[] GetGenericArguments(Type type)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsGenericType)
            return typeInfo.GenericTypeParameters;
#else
        if (type.IsGenericType)
            return type.GetGenericArguments();
#endif
        return [];
    }
}
