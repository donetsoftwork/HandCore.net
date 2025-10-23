namespace Hand.Maping;

/// <summary>
/// 指定映射目标类型
/// </summary>
/// <param name="destType"></param>
public class MapToAttribute(Type destType)
{
    /// <summary>
    /// 目标类型
    /// </summary>
    public Type DestType { get; } = destType;
}
