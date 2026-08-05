using System.Text;

namespace Hand.Words;

/// <summary>
/// 小驼峰(首字母小写‌‌)
/// </summary>
public class CamelWordRule : IWordRule
{
    private CamelWordRule() { }
    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
    {
        // 小驼峰首字母小写
        if (depth == 0)
            builder.Append(char.ToLowerInvariant(first));
        else
            builder.Append(char.ToUpperInvariant(first));
    }
    /// <summary>
    /// 首字母小写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string FistToLower(ReadOnlySpan<char> original)
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
            if (char.IsUpper(first))
            {
                ReadOnlySpan<char> list = [char.ToLowerInvariant(first), .. others];
                return list.ToString();
            }
            else if (char.IsLower(first))
            {
                ReadOnlySpan<char> list = [first, .. others];
                return list.ToString();
            }
            return FistToLower(others);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        }
        return string.Empty;
#endif
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new CamelWordRule();
}
