using Hand.Comparers;
using Hand.Rule.Configurations;

namespace Hand;

/// <summary>
/// 规则解析器
/// </summary>
/// <param name="include"></param>
/// <param name="exclude"></param>
/// <param name="separators"></param>
/// <param name="memberComparer"></param>
public class MemberRuleParser(string include, string exclude, char[] separators, StringComparer memberComparer)
{
    #region 配置
    /// <summary>
    /// 包含标记
    /// </summary>
    protected readonly string _includePrefix = include;
    /// <summary>
    /// 排除标记
    /// </summary>
    protected readonly string _excludePrefix = exclude;
    /// <summary>
    /// 分割符
    /// </summary>
    protected readonly char[] _separators = separators;
    /// <summary>
    /// 成员比较
    /// </summary>
    protected readonly StringComparer _memberComparer = memberComparer;

    /// <summary>
    /// 包含标记
    /// </summary>
    public string IncludePrefix
        => _includePrefix;
    /// <summary>
    /// 排除标记
    /// </summary>
    public string ExcludePrefix
        => _excludePrefix;
    /// <summary>
    /// 分割符
    /// </summary>
    public char[] Separators
        => _separators;
    /// <summary>
    /// 成员比较
    /// </summary>
    public StringComparer MemberComparer
        => _memberComparer;
    #endregion
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public ValidationConfiguration<string> Parse(string? text)
    {
        var comparison = CompareConverter.ToComparison(_memberComparer);
        if (string.IsNullOrWhiteSpace(text) || text!.Equals("ALL", comparison))
            return AllConfiguration<string>.Instance;
        if (text.Equals("Empty", comparison))
            return EmptyConfiguration<string>.Instance;
        var includes = new HashSet<string>(_memberComparer);
        var excludes = new HashSet<string>(_memberComparer);
        HashSet<string> items = includes;
        var parts = text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (part.StartsWith(_includePrefix, comparison))
            {
                items = includes;
            }
            else if (part.StartsWith(_excludePrefix, comparison))
            {
                items = excludes;
            }
            else
            {
                items.Add(part);
            }
        }
        if (includes.Count > 0)
        {
            if(excludes.Count > 0)
            {
                return new ComplexConfiguration<string>(includes, excludes);
            }
            else
            {
                return new IncludeConfiguration<string>(includes);
            }
        }
        else if (excludes.Count > 0)
        {
            return new ExcludeConfiguration<string>(excludes);
        }
        else
        {
            return EmptyConfiguration<string>.Instance;
        }
    }
    /// <summary>
    /// 默认实例
    /// </summary>
    public static MemberRuleParser Default
        => DefaultInner.Instance;
    #region DefaultInner
    class DefaultInner
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        internal static readonly MemberRuleParser Instance = new("Include:", "Exclude:", [' '], StringComparer.OrdinalIgnoreCase);
    }
    #endregion
}
