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
    public static string FistToUpper(string original)
    {
        if (string.IsNullOrEmpty(original))
            return string.Empty;
        var first = original[0];
        if (char.IsLower(first))
        {
            var builder = new StringBuilder(original);
            builder[0] = char.ToUpperInvariant(first);
            return builder.ToString();
        }
        return original;
    }
    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string FistToUpper(ReadOnlySpan<char> original)
    {
        var count = original.Length;
        if (count == 0)
            return string.Empty;
        var first = original[0];
        if (char.IsUpper(first))
        {
            var builder = new StringBuilder(count);
            builder.Append(char.ToUpperInvariant(first));
            for (var i = 1; i < count; i++)
                builder.Append(original[i]);
            return builder.ToString();
        }
        return original.ToString();
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new PascalWordRule();
}
