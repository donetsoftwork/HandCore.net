namespace Hand.Converters;

/// <summary>
/// 命名转化接口
/// </summary>
public interface INameConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string Convert(string name);
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string Convert(ReadOnlySpan<char> name);
}
