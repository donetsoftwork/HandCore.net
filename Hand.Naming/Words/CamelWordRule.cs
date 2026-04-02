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
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public static string FistToLower(string original, int startIndex = 0)
    {
        if (string.IsNullOrEmpty(original))
            return string.Empty;
        var count = original.Length;
        var first = original[startIndex];
        if (char.IsUpper(first))
        {
            var builder = new StringBuilder(count - startIndex);
            builder.Append(char.ToLowerInvariant(first));
            for (var i = startIndex + 1; i < count; i++)
                builder.Append(original[i]);
            return builder.ToString();
        }
        return original;
    }
    /// <summary>
    /// 首字母小写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string FistToLower(ReadOnlySpan<char> original)
    {
        var count = original.Length;
        if (count == 0)
            return string.Empty;
        var first = original[0];
        if (char.IsUpper(first))
        {
            var builder = new StringBuilder(count);
            builder.Append(char.ToLowerInvariant(first));
            for (var i = 1; i < count; i++)
                builder.Append(original[i]);
            return builder.ToString();
        }
        return original.ToString();
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new CamelWordRule();
}
