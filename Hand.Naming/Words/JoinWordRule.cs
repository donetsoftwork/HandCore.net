using System.Text;

namespace Hand.Words;

/// <summary>
/// 原始规则
/// </summary>
/// <param name="separator"></param>
/// <param name="original"></param>
public class JoinWordRule(string separator, IWordRule original)
    : IWordRule
{
    private readonly string _separator = separator;
    private readonly IWordRule _original = original;

    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
    {
        if (depth > 0)
            builder.Append(_separator);
        _original.CheckFirst(builder, first, depth);
    }
}
