using Hand.Rule;

namespace Hand.Paths;

/// <summary>
/// 默认单词分割规则
/// </summary>
/// <param name="separators"></param>
public class DefaultSplitRule(IEnumerable<char> separators)
    : IValidation<char>
{
    private readonly HashSet<char> _separatorSet = [.. separators];
    /// <inheritdoc />
    public bool Validate(char item)
        => _separatorSet.Contains(item);
}
