using Hand.Rule;
using Hand.Words;

namespace Hand.Paths;

/// <summary>
/// 帕斯卡拆分单词规则(每个单词首字母大写)
/// </summary>
public class PascalSplitRule : IValidation<char>
{
    /// <summary>
    /// 保护构造函数
    /// </summary>
    protected PascalSplitRule() { }
    /// <inheritdoc />
    public bool Validate(char item)
        => char.IsUpper(item);
    /// <summary>
    /// 验证器单例
    /// </summary>
    public static readonly IValidation<char> Validation = new PascalSplitRule();
}
