using System.Text;

namespace Hand.Words;

/// <summary>
/// 小写规则(每个单词首字母小写)
/// </summary>
public class LowerWordRule : IWordRule
{
    private LowerWordRule() { }
    /// <inheritdoc />
    public void CheckFirst(StringBuilder builder, char first, int depth)
        => builder.Append(char.ToLowerInvariant(first));
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly IWordRule Instance = new LowerWordRule();
}
