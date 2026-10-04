using Hand.Primitives;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Hand.Enumerations;

/// <summary>
/// 枚举服务
/// </summary>
public static partial class ReflectionEnumeration
{
    #region Dictionary
    #region Enum
    /// <summary>
    /// 反射枚举类型为字典
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<string, Enumeration> GetEnumDictionary<TEnum>(IEqualityComparer<string> comparer)
        where TEnum : Enum
    {
        var names = new Dictionary<string, Enumeration>(comparer);
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            var name = field.Name;
            var original = System.Convert.ToInt64(field.GetValue(null));
            var description = field.GetCustomAttribute<DescriptionAttribute>();
            var item = new Enumeration(name, original, description?.Description ?? string.Empty);
            names[name] = item;
            var member = field.GetCustomAttribute<EnumMemberAttribute>();
            CheckEnumMember(names, member, item);
        }
        return names;
    }
    /// <summary>
    /// 反射位标记枚举类型为字典
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static (List<FlagEnumeration>, List<FlagEnumeration>, IDictionary<string, FlagEnumeration>, IDictionary<long, FlagEnumeration>) GetFlagEnumDictionary<TEnum>(IEqualityComparer<string> comparer)
        where TEnum : Enum
    {
        var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
        var count = fields.Length;
        var names = new Dictionary<string, FlagEnumeration>(count, comparer);
        var originals = new Dictionary<long, FlagEnumeration>(count);
        var items = new List<FlagEnumeration>(count);
        var flags = new List<FlagEnumeration>(count);
        foreach (var field in fields)
        {
            var name = field.Name;
            var original = System.Convert.ToInt64(field.GetValue(null));
            var member = field.GetCustomAttribute<EnumMemberAttribute>();
            if (originals.TryGetValue(original, out var enumeration))
            {
                // 处理枚举别名
                names[name] = enumeration;
                CheckEnumMember(names, member, enumeration);
                continue;
            }
            FlagEnumeration item;
            if (original == 0L)
            {
                item = CreateFlag(comparer, name, 0L, field.GetCustomAttribute<DescriptionAttribute>());
            }
            else if (FlagEnumeration.VerifyFlag(original))
            {
                item = CreateFlag(comparer, name, original, field.GetCustomAttribute<DescriptionAttribute>());
                flags.Add(item);
            }
            else
            {
                var itemNames = new HashSet<string>(comparer) { name };
                CheckFlags(flags, original, itemNames);
                var description = field.GetCustomAttribute<DescriptionAttribute>();
                item = new FlagEnumeration(itemNames, name, original, description?.Description ?? string.Empty);
            }
            CheckEnumMember(names, member, item);
            originals.Add(original, item);
            names[name] = item;
            items.Add(item);
        }
        return (items, flags, names, originals);
    }
    #endregion
    /// <summary>
    /// 构建位标记枚举
    /// </summary>
    /// <param name="comparer"></param>
    /// <param name="name"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FlagEnumeration CreateFlag(IEqualityComparer<string> comparer, string name, long original, DescriptionAttribute? description)
        => new(comparer, name, original, description?.Description ?? string.Empty);
    /// <summary>
    /// 检查标记位
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="names"></param>
    private static void CheckFlags(List<FlagEnumeration> flags, long original, HashSet<string> names)
    {
        foreach (var item in flags)
        {
            var flag = item.Original;
            if ((flag & original) == flag)
                names.Add(item.Name);
        }
    }
    /// <summary>
    /// 反射枚举类为字典
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <returns></returns>
    public static IDictionary<string, TEnumeration> GetEnumerationDictionary<TEnumeration>(IEqualityComparer<string>? comparer = null)
        where TEnumeration : IEnumeration
    {
        var names = new Dictionary<string, TEnumeration>(comparer ?? StringComparer.Ordinal);
        var fields = typeof(TEnumeration).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            if (field.GetValue(null) is TEnumeration item)
                names[field.Name] = item;
        }
        var properties = typeof(TEnumeration).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (var property in properties)
        {
            if (property.CanRead && property.GetValue(null) is TEnumeration item)
                names[property.Name] = item;
        }
        return names;
    }
    #endregion
    /// <summary>
    /// 处理EnumMember标记
    /// </summary>
    /// <param name="names"></param>
    /// <param name="member"></param>
    /// <param name="enumeration"></param>
    public static void CheckEnumMember<TEnumeration>(IDictionary<string, TEnumeration> names, EnumMemberAttribute? member, TEnumeration enumeration)
    {
        if (member is null)
            return;
        var memberName = member.Value;
        if (string.IsNullOrEmpty(memberName))
            return;
        names[memberName] = enumeration;
    }
    /// <summary>
    /// 获取枚举提供者
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static EnumerationProvider<Enumeration> GetEnumProvider<TEnum>(IEqualityComparer<string>? comparer = null)
        where TEnum : Enum
        => new(GetEnumDictionary<TEnum>(comparer ?? StringComparer.Ordinal));
    /// <summary>
    /// 获取枚举提供者
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static FlagEnumerationProvider GetFlagEnumProvider<TEnum>(IEqualityComparer<string>? comparer = null)
        where TEnum : struct, Enum
    {
        var(items, flags, names, originals) = GetFlagEnumDictionary<TEnum>(comparer ?? StringComparer.Ordinal);
        return new(items, flags, names, originals);
    }
    /// <summary>
    /// 获取枚举提供者
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static EnumerationProvider<TEnumeration> GetEnumerationProvider<TEnumeration>(IEqualityComparer<string>? comparer = null)
        where TEnumeration : IEnumeration
        => new(GetEnumerationDictionary<TEnumeration>(comparer));
    /// <summary>
    /// 获取枚举提供者
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static FlagReflectionProvider<TEnumeration> GetFlagEnumerationProvider<TEnumeration>(IEqualityComparer<string>? comparer = null)
         where TEnumeration : IFlagEnumeration
        => new(GetEnumerationDictionary<TEnumeration>(comparer));
}
