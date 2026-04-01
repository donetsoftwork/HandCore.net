using System.Text;

namespace Hand.Words;

/// <summary>
/// 下划线开头(字段命名)
/// </summary>
public class UnderWordRule : IWordRule
{
    /// <summary>
    /// 下换线
    /// </summary>
    public const char Prefix = '_';
    private UnderWordRule() { }
    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
    {
        // 下划线开头
        if (first == Prefix)
            builder.Append(Prefix);
        else if (depth == 0)
            builder.Append(Prefix).Append(char.ToLowerInvariant(first));
        else
            builder.Append(char.ToUpperInvariant(first));
    }
    /// <summary>
    /// 下换线
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string Under(string original)
    {
        if (string.IsNullOrEmpty(original))
            return "_";
        var builder = new StringBuilder(original.Length + 1);
        builder.Append(Prefix)
            .Append(original);
        return builder.ToString();
    }
    /// <summary>
    /// 下换线
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string Under(ReadOnlySpan<char> original)
    {
        var count = original.Length;
        if (count == 0)
            return "_";
        var builder = new StringBuilder(count + 1);
        builder.Append(Prefix);
        for (var i = 0; i < count; i++)
            builder.Append(original[i]);
        return builder.ToString();
    }
    /// <summary>
    /// 下换线次字母小写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string UnderLower(string original)
    {
        if (string.IsNullOrEmpty(original))
            return "_";
        var builder = new StringBuilder(original.Length + 1)
            .Append(Prefix)
            .Append(original);
        var first = original[0];
        if (char.IsUpper(first))
            builder[1] = char.ToLowerInvariant(first);
        return builder.ToString();
    }
    /// <summary>
    /// 下换线次字母小写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string UnderLower(ReadOnlySpan<char> original)
    {
        var count = original.Length;
        if (count == 0)
            return "_";
        var builder = new StringBuilder(count + 1)
            .Append(Prefix);
        var first = original[0];
        if (char.IsUpper(first))
        {
            builder.Append(char.ToLowerInvariant(first));
        }
        else
        {
            builder.Append(first);
        }
        for (var i = 1; i < count; i++)
            builder.Append(original[i]);
        return builder.ToString();
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new UnderWordRule();
}
