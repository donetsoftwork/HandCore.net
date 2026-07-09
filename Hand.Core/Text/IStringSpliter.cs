namespace Hand.Text;

/// <summary>
/// 字符串拆分器
/// </summary>
public interface IStringSpliter
{
    /// <summary>
    /// 拆分字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    IEnumerable<string> Split(ReadOnlySpan<char> str);
}
