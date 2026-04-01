using System.Text;

namespace Hand.Words;

/// <summary>
/// 单词默认规则
/// </summary>
public class DefaultWordRule
    : IWordRule
{
    private DefaultWordRule() { }
    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
        => builder.Append(first);
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new DefaultWordRule();
}
