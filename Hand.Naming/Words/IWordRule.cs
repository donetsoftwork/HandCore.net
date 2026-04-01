using System.Text;

namespace Hand.Words;

/// <summary>
/// 单词规则
/// </summary>
public interface IWordRule
{
    /// <summary>
    /// 首字母处理
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="first"></param>
    /// <param name="depth"></param>
    void CheckFirst(StringBuilder builder, char first, int depth);
}
