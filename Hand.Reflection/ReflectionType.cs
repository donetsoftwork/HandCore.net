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
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    #endregion
    #region HasGenericType
    /// <summary>
    /// 判断是否包含泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool HasGenericType(Type type, Type genericType)
    {
        if (IsGenericType(type, genericType))
            return true;
        var interfaces = type.GetInterfaces();
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
    {
        if (IsGenericType(type, genericInterface))
        {
            yield return type;
            yield break;
        }
        var interfaces = type.GetInterfaces();
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
        {
            ScanInterfaceImp(Assembly.GetExecutingAssembly(), action, interfaces);
            return;
        }
        var types = assemblies.SelectMany(assembly => assembly.GetTypes()
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
        var types = assembly.GetTypes();
        ScanInterfaceImp(interfaces, types.Where(type => !type.IsInterface && !type.IsAbstract), action);
    }

    /// <summary>
    /// 扫描接口实现
    /// </summary>
    /// <param name="interfaces"></param>
    /// <param name="types"></param>
    /// <param name="action"></param>
    public static void ScanInterfaceImp(IEnumerable<Type> interfaces, IEnumerable<Type> types, Action<Type, Type> action)
    {
        foreach (var @interface in interfaces)
        {
            if (@interface.IsGenericTypeDefinition)
            {
                ScanGenericInterfaceImp(@interface, types, action);
            }
            else
            {
                foreach (var type in types)
                {
                    var isImpl = PairTypeKey.CheckValueType(type, @interface);
                    if (isImpl)
                        action(@interface, type);
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
    public static void ScanGenericInterfaceImp(Type genericInterface, IEnumerable<Type> types, Action<Type, Type> action)
    {
        var interfaceParamCount = genericInterface.GetGenericArguments().Length;
        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition)
            {
                var typeParamCount = type.GetGenericArguments().Length;
                // 泛型实现
                if (typeParamCount == interfaceParamCount
                    && IsGenericType(type, genericInterface))
                {
                    action(genericInterface, type);
                }
            }
            else
            {
                // 泛型接口闭合实现
                foreach (var @interface in GetGenericCloseInterfaces(type, genericInterface))
                {
                    action(@interface, type);
                }
            }
        }
    }
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
        if (type.IsGenericType)
            return type.GetGenericArguments();
        return [];
    }
}
