using System.Text;

namespace Hand.Words;

/// <summary>
/// 帕斯卡命名规则(每个单词首字母大写)
/// </summary>
public class PascalWordRule : IWordRule
{
    private PascalWordRule() { }
    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
        => builder.Append(char.ToUpperInvariant(first));
    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string FistToUpper(ReadOnlySpan<char> original)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        if (original is [var first, .. var others])
        {
#else
        var count = original.Length;
        if (count == 0)
            return string.Empty;

        var first = original[0];
        var others = original.Slice(1);
#endif
            if (char.IsLower(first))
            {
                ReadOnlySpan<char> list = [char.ToUpperInvariant(first), .. others];
                return list.ToString();
            }
            else if (char.IsUpper(first))
            {
                ReadOnlySpan<char> list = [first, .. others];
                return list.ToString();
            }
            return FistToUpper(others);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        }
        return string.Empty;
#endif
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new PascalWordRule();
}
