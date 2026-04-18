using System;
using System.Collections.Generic;
using System.Linq;

namespace Hand.Rule;

/// <summary>
/// 规则解析器
/// </summary>
/// <param name="include"></param>
/// <param name="exclude"></param>
/// <param name="separators"></param>
/// <param name="memberComparer"></param>
public class MemberRuleParser(string include, string exclude, char[] separators, IEqualityComparer<string> memberComparer)
{
    #region 配置
    private readonly string _includePrefix = include;
    private readonly string _excludePrefix = exclude;
    private readonly char[] _separators = separators;
    private readonly IEqualityComparer<string> _memberComparer = memberComparer;

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
    public IEqualityComparer<string> MemberComparer
        => _memberComparer;
    #endregion
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public IValidation<string> Parse(string? text)
    {
        if (string.IsNullOrWhiteSpace(text) || text!.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            return Logic.True<string>();
        if (text!.Equals("Empty", StringComparison.OrdinalIgnoreCase))
            return Logic.False<string>();
        if (text.StartsWith(_includePrefix, StringComparison.OrdinalIgnoreCase))
            return ToIncluded(text, _separators, _memberComparer, 1);
        // 逐个排除
        if (text.StartsWith(_excludePrefix, StringComparison.OrdinalIgnoreCase))
            return ToIncluded(text, _separators, _memberComparer, 1).Not();
        // 逐个解析
        return ToIncluded(text, _separators, _memberComparer, 0);
    }
    /// <summary>
    /// 转化为被包含验证规则
    /// </summary>
    /// <param name="text"></param>
    /// <param name="separators"></param>
    /// <param name="comparer"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public static IValidation<string> ToIncluded(string text, char[] separators, IEqualityComparer<string> comparer, int skip = 0)
    {
        var items = skip > 0 ? text.Split(separators, StringSplitOptions.RemoveEmptyEntries).Skip(skip).Distinct() :
            text.Split(separators, StringSplitOptions.RemoveEmptyEntries).Distinct();
        return Logic.Included(comparer, items);
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

        internal static readonly MemberRuleParser Instance = new("Include:", "Exclude:", [' '], StringComparer.Ordinal);
    }
    #endregion
}
