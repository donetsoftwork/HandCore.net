namespace Hand.Naming;

/// <summary>
/// 命名转化接口
/// </summary>
public interface INameConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    string Convert(string name, int startIndex = 0);
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string Convert(ReadOnlySpan<char> name);
}
