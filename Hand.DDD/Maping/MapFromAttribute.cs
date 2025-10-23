namespace Hand.Maping;

/// <summary>
/// 指定映射来源类型
/// </summary>
/// <param name="sourceType"></param>
public class MapFromAttribute(Type sourceType)
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public Type SourceType { get; } = sourceType;
}
