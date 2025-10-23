namespace Hand.Maping;

/// <summary>
/// 指定映射目标类型
/// </summary>
/// <typeparam name="TDest"></typeparam>
public class MapToAttribute<TDest>()
    : MapToAttribute(typeof(TDest))
{
}
