namespace Hand.Paths;

/// <summary>
/// 路径转化规则
/// </summary>
public interface IPathRule
{
    /// <summary>
    /// 拆分
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    IEnumerable<string> Split‌(string fullPath, int startIndex = 0);
    /// <summary>
    /// 拆分
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    IEnumerable<string> Split‌(ReadOnlySpan<char> fullPath);
}
