namespace Hand.Maping;

/// <summary>
/// 指定映射来源类型
/// </summary>
/// <typeparam name="TSource"></typeparam>
public class MapFromAttribute<TSource>()
    : MapFromAttribute(typeof(TSource))
{
}
