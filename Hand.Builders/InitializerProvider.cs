using Hand.Cache;
using Hand.Members;
using Hand.Reflection;
using System.Reflection;

namespace Hand;

/// <summary>
/// 初始化信息缓存
/// </summary>
/// <param name="parameterCountDesc"></param>
public class InitializerProvider(bool parameterCountDesc)
    : CacheFactoryBase<Type, InitializerInfo?>()
{
    private readonly bool _parameterCountDesc = parameterCountDesc;
    /// <summary>
    /// 获取参数最多的还是最少的
    /// </summary>
    public bool ParameterCountDesc 
        => _parameterCountDesc;
    /// <inheritdoc />
    protected override InitializerInfo? CreateNew(in Type key)
    {
        var constructor = ReflectionMember.GetConstructor(key, _parameterCountDesc);
        if (constructor is null)
            return null;
        var parameters = constructor.GetParameters()
            .ToDictionary(static item => item.Name!, StringComparer.OrdinalIgnoreCase);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var names = parameters.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
#else
        var names = new HashSet<string>(parameters.Keys, StringComparer.OrdinalIgnoreCase);
#endif
        var properties = GetProperties(key, names);
        var fields = GetFields(key, names);

        return new(constructor, parameters, properties, fields);
    }
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="ignoreNames"></param>
    /// <returns></returns>
    public static IDictionary<string, PropertyInfo> GetProperties(Type instanceType, ISet<string> ignoreNames)
    {
        var properties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var name = property.Name;
            if (ignoreNames.Contains(name))
                continue;
            properties.Add(name, property);
            ignoreNames.Add(name);
        }
        return properties;
    }
    /// <summary>
    /// 获取字段
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="ignoreNames"></param>
    /// <returns></returns>
    public static IDictionary<string, FieldInfo> GetFields(Type instanceType, ISet<string> ignoreNames)
    {
        var fields = new Dictionary<string, FieldInfo>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in instanceType.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            var name = field.Name;
            if (ignoreNames.Contains(name))
                continue;
            fields.Add(name, field);
            ignoreNames.Add(name);
        }
        return fields;
    }
    /// <summary>
    /// 优先获取参数少的
    /// </summary>
    public static InitializerProvider Asc
        => AscInner.Asc;
    /// <summary>
    /// 优先获取参数多的
    /// </summary>
    public static InitializerProvider Desc
        => DescInner.Desc;

    static class AscInner
    {
        internal static readonly InitializerProvider Asc = new(true);
    }
    static class DescInner
    {
        internal static readonly InitializerProvider Desc = new(true);
    }

}
