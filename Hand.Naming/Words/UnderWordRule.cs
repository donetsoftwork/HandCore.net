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
    ///// <summary>
    ///// 下换线
    ///// </summary>
    ///// <param name="original"></param>
    ///// <param name="startIndex"></param>
    ///// <returns></returns>
    //public static string Under(string original, int startIndex = 0)
    //{
    //    if (string.IsNullOrEmpty(original))
    //        return "_";
    //    var first = original[startIndex];
    //    if (first == Prefix)
    //        return original.Substring(startIndex);
    //    var count = original.Length;
    //    var builder = new StringBuilder(count + 1 - startIndex);
    //    builder.Append(Prefix);
    //    for (var i = startIndex; i < count; i++)
    //        builder.Append(original[i]);
    //    return builder.ToString();
    //}
    ///// <summary>
    ///// 下换线
    ///// </summary>
    ///// <param name="original"></param>
    ///// <returns></returns>
    //public static string Under(ReadOnlySpan<char> original)
    //{
    //    var count = original.Length;
    //    if (count == 0)
    //        return "_";
    //    var first = original[0];
    //    if (first == Prefix)
    //        return original.ToString();
    //    var builder = new StringBuilder(count + 1);
    //    builder.Append(Prefix);
    //    for (var i = 0; i < count; i++)
    //        builder.Append(original[i]);
    //    return builder.ToString();
    //}
    /// <summary>
    /// 下换线次字母小写
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string UnderLower(ReadOnlySpan<char> original)
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
                ReadOnlySpan<char> list = [Prefix, char.ToLowerInvariant(first), .. others];
                return list.ToString();
            }
            else if (char.IsLower(first))
            {
                ReadOnlySpan<char> list = [Prefix, first, .. others];
                return list.ToString();
            }
            return UnderLower(others);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        }
        return string.Empty;
#endif
    }
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new UnderWordRule();
}
