using System.Reflection;
using System.Runtime.CompilerServices;

namespace Hand.Reflection;

/// <summary>
/// 反射成员
/// </summary>
public static class ReflectionMember
{
    #region GetProperties
    /// <summary>
    /// 筛选属性
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<PropertyInfo> GetProperties<T>(Func<PropertyInfo, bool> filter, BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        => typeof(T).GetProperties(binding).Where(filter);
    /// <summary>
    /// 筛选属性
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<PropertyInfo> GetProperties(Type declareType, Func<PropertyInfo, bool> filter, BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        => declareType.GetProperties(binding).Where(filter);
    #endregion
    #region GetFields
    /// <summary>
    /// 筛选字段
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<FieldInfo> GetFields<T>(Func<FieldInfo, bool> filter, BindingFlags binding = BindingFlags.Static | BindingFlags.Public)
        => typeof(T).GetFields(binding).Where(filter);
    /// <summary>
    /// 筛选字段
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(Type declareType, Func<FieldInfo, bool> filter, BindingFlags binding = BindingFlags.Static | BindingFlags.Public)
        => declareType.GetFields(binding).Where(filter);
    #endregion
    //#region Properties
    ///// <summary>
    ///// 获取所有属性
    ///// </summary>
    ///// <typeparam name="TStructuralType"></typeparam>
    ///// <returns></returns>
    //public static PropertyInfo[] GetProperties<TStructuralType>()
    //    => GetProperties(typeof(TStructuralType));
    ///// <summary>
    ///// 获取所有属性
    ///// </summary>
    ///// <returns></returns>
    //public static PropertyInfo[] GetProperties(Type declareType)
    //    => declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    //#endregion
    //#region GetFields
    ///// <summary>
    ///// 获取所有实例字段
    ///// </summary>
    ///// <typeparam name="TStructuralType"></typeparam>
    ///// <returns></returns>
    //public static FieldInfo[] GetFields<TStructuralType>()
    //    => GetFields(typeof(TStructuralType));
    ///// <summary>
    ///// 获取所有实例字段
    ///// </summary>
    ///// <returns></returns>
    //public static FieldInfo[] GetFields(Type declareType)
    //    => declareType.GetFields(BindingFlags.Instance | BindingFlags.Public);
    //#endregion
    //#region GetStaticFields
    ///// <summary>
    ///// 获取所有静态字段
    ///// </summary>
    ///// <typeparam name="TStructuralType"></typeparam>
    ///// <returns></returns>
    //public static FieldInfo[] GetStaticFields<TStructuralType>()
    //    => GetStaticFields(typeof(TStructuralType));
    ///// <summary>
    ///// 获取所有静态字段
    ///// </summary>
    ///// <returns></returns>
    //public static FieldInfo[] GetStaticFields(Type declareType)
    //    => declareType.GetFields(BindingFlags.Static | BindingFlags.Public);
    //#endregion
    #region ConstructorInfo
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="parameterCountDesc"></param>
    /// <returns></returns>
    public static ConstructorInfo? GetConstructor(Type instanceType, bool parameterCountDesc)
    {
        return SortByParameterCount(instanceType.GetConstructors(), parameterCountDesc)
            .FirstOrDefault();
    }
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="list"></param>
    /// <param name="parameterCountDesc"></param>
    /// <returns></returns>
    public static IEnumerable<TMethod> SortByParameterCount<TMethod>(IEnumerable<TMethod> list, bool parameterCountDesc)
         where TMethod : MethodBase
    {
        if (parameterCountDesc)
            return list.OrderByDescending(c => c.GetParameters().Length);
        return list.OrderBy(c => c.GetParameters().Length);
    }
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="parameterType"></param>
    /// <returns></returns>
    public static ConstructorInfo? GetConstructorByParameterType(Type declareType, Type parameterType)
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
    public static ConstructorInfo? GetConstructor(Type declareType, Func<ParameterInfo[], bool> filter)
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
