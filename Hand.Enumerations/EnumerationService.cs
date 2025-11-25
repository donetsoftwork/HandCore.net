using Hand.Reflection;

namespace Hand.Enumerations;

/// <summary>
/// 枚举服务
/// </summary>
public static class EnumerationService
{
    /// <summary>
    /// 获取枚举的所有值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetAll<T>()
        where T : EnumerationBase
    {
        var fields = ReflectionMember.GetStaticFields<T>();
        foreach (var field in fields)
        {
            var instance = field.GetValue(null);
            if (instance is T item)
                yield return item;
        }
    }
    /// <summary>
    /// 通过值获取枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T FromValue<T>(int value)
        where T : EnumerationBase
    {
        return Match(GetAll<T>(), value, "value", item => item.Value == value);
    }
    /// <summary>
    /// 通过展示获取枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="code"></param>
    /// <returns></returns>
    public static T FromCode<T>(string code)
        where T : EnumerationBase
    {
        return Match(GetAll<T>(), code, nameof(code), item => item.Code == code);
    }
    /// <summary>
    /// 通过展示获取枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="display"></param>
    /// <returns></returns>
    public static T FromDisplay<T>(string display)
        where T : EnumerationBase
    {
        return Match(GetAll<T>(), display, nameof(display), item => item.Display == display);
    }
    /// <summary>
    /// 匹配
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="list"></param>
    /// <param name="key"></param>
    /// <param name="description"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T Match<T, K>(this IEnumerable<T> list, K key, string description, Func<T, bool> predicate)
        where T : EnumerationBase
    {
        var item = list.FirstOrDefault(predicate);
        if (item is null)
            throw new Exception($"'{key}' is not a valid {description} in {typeof(T)}");

        return item;
    }
}
