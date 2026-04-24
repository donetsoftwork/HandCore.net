using Hand.Comparers;
using Hand.Maping;
using Hand.Maping.Recognizers;
using Hand.Rule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hand;

/// <summary>
/// 成员识别解析器
/// </summary>
/// <param name="cross"></param>
/// <param name="through"></param>
/// <param name="filter"></param>
/// <param name="include"></param>
/// <param name="exclude"></param>
/// <param name="separators"></param>
/// <param name="factory"></param>
/// <param name="comparison"></param>
public class MemberRecognizeParser(string cross, string through, string filter, string include, string exclude, char[] separators, Dictionary<string, Func<IEnumerable<string>, IProjection<string>?>> factory, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    : MemberRuleParser(include, exclude, separators, CompareConverter.ToComparer(comparison))
{
    #region 配置
    private readonly string _crossPrefix = cross;
    private readonly string _throughPrefix = through;
    private readonly string _filterPrefix = filter;
    private readonly StringComparison _comparison = comparison;
    private readonly Dictionary<string, Func<IEnumerable<string>, IProjection<string>?>> _factorys = factory;
    #endregion
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public new IRecognizer<string> Parse(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return RecognizerInner.Default;
        var parts = text!.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return RecognizerInner.Default;
        if (text.StartsWith(_crossPrefix, StringComparison.OrdinalIgnoreCase))
        {
            if(TryParseProjection(parts, 1, out var projection))
                return new CrossRecognizer<string>(projection!, _memberComparer);
        }
        else if (text.StartsWith(_throughPrefix, StringComparison.OrdinalIgnoreCase))
        {
            if (TryParseProjection(parts, 1, out var projection))
                return new ThroughRecognizer<string>(projection!, _memberComparer);
        }
        else if (text.StartsWith(_filterPrefix, StringComparison.OrdinalIgnoreCase))
        {
            if (TryParseProjection(parts, 1, out var projection))
                return new FilterRecognizer<string>(projection!, _memberComparer);
            if(TryParseFilter(parts, 1, out var filter))
                return new ValidationRecognizer<string>(filter!, _memberComparer);
        }
        else if (TryParseProjection(parts, 0, out var projection))
        {
            return new ThroughRecognizer<string>(projection!, _memberComparer);
        }
        else if (TryParseFilter(parts, 0, out var filter))
        {
            return new ValidationRecognizer<string>(filter!, _memberComparer);
        }            
        return RecognizerInner.Default;
    }
    /// <summary>
    /// 解析过滤器
    /// </summary>
    /// <param name="parts"></param>
    /// <param name="start"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public bool TryParseFilter(string[] parts, int start, out IValidation<string>? filter)
    {
        if (start >= parts.Length)
        {
            filter = null;
            return false;
        }
        var name = parts[start];
        if (name.Equals(_includePrefix, StringComparison.OrdinalIgnoreCase))
            filter = ToIncluded(parts, _memberComparer, start + 1);
        // 逐个排除
        else if (name.Equals(_excludePrefix, StringComparison.OrdinalIgnoreCase))
            filter = ToIncluded(parts, _memberComparer, start + 1).Not();
        else
            // 逐个解析
            filter = ToIncluded(parts, _memberComparer, start);
        return true;
    }
    /// <summary>
    /// 解析投影
    /// </summary>
    /// <param name="parts"></param>
    /// <param name="start"></param>
    /// <param name="projection"></param>
    /// <returns></returns>
    public bool TryParseProjection(string[] parts, int start, out IProjection<string>? projection)
    {
        var skip = start + 1;
        if (skip >= parts.Length)
        {
            projection = null;
            return false;
        }
        var name = parts[start];
        if (_factorys.TryGetValue(name, out var factory))
        {
            projection = factory(parts.Skip(skip));
            return projection is not null;
        }
        projection = null;
        return false;
    }

    #region Use
    /// <summary>
    /// 添加投影
    /// </summary>
    /// <param name="name"></param>
    /// <param name="factory"></param>
    public MemberRecognizeParser UseProjection(string name, Func<IEnumerable<string>, IProjection<string>?> factory)
    {
        _factorys[name] = factory;
        return this;
    }
    /// <summary>
    /// 添加前缀投影
    /// </summary>
    /// <returns></returns>
    public MemberRecognizeParser UsePrefix()
        => UseProjection(nameof(Prefix), arguments => Prefix(arguments, _comparison));
    /// <summary>
    /// 添加后缀投影
    /// </summary>
    /// <returns></returns>
    public MemberRecognizeParser UseSuffix()
        => UseProjection(nameof(Suffix), arguments => Suffix(arguments, _comparison));
    /// <summary>
    /// 添加前缀去除投影
    /// </summary>
    /// <returns></returns>
    public MemberRecognizeParser UseRemovePrefix()
        => UseProjection(nameof(RemovePrefix), arguments => RemovePrefix(arguments, _comparison));
    /// <summary>
    /// 添加后缀去除投影
    /// </summary>
    /// <returns></returns>
    public MemberRecognizeParser UseRemoveSuffix()
        => UseProjection(nameof(RemoveSuffix), arguments => RemoveSuffix(arguments, _comparison));
    #endregion
    #region Projection
    /// <summary>
    /// 解析前缀投影
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static IProjection<string>? Prefix(IEnumerable<string> arguments, StringComparison comparison)
    {
        var prefix = arguments.FirstOrDefault();
        if (string.IsNullOrEmpty(prefix))
            return null;
        return new PrefixProjection(prefix, comparison);
    }
    /// <summary>
    /// 解析后缀投影
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static IProjection<string>? Suffix(IEnumerable<string> arguments, StringComparison comparison)
    {
        var suffix = arguments.FirstOrDefault();
        if (string.IsNullOrEmpty(suffix))
            return null;
        return new SuffixProjection(suffix, comparison);
    }
    /// <summary>
    /// 解析前缀去除投影
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static IProjection<string>? RemovePrefix(IEnumerable<string> arguments, StringComparison comparison)
    {
        var enumerator = arguments.GetEnumerator();
        if (!enumerator.MoveNext())
            return null;
        var prefix = enumerator.Current;
        if (string.IsNullOrEmpty(prefix))
            return null;
        if (enumerator.MoveNext())
            return new ReplacePrefixProjection(prefix, enumerator.Current, comparison);
        return new RemovePrefixProjection(prefix, comparison);
    }
    /// <summary>
    /// 解析后缀去除投影
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static IProjection<string>? RemoveSuffix(IEnumerable<string> arguments, StringComparison comparison)
    {
        var enumerator = arguments.GetEnumerator();
        if (!enumerator.MoveNext())
            return null;
        var suffix = enumerator.Current;
        if (string.IsNullOrEmpty(suffix))
            return null;
        if (enumerator.MoveNext())
            return new ReplaceSuffixProjection(suffix, enumerator.Current, comparison);
        return new RemoveSuffixProjection(suffix, comparison);
    }
    /// <summary>
    /// 构造默认实例
    /// </summary>
    /// <param name="cross"></param>
    /// <param name="through"></param>
    /// <param name="filter"></param>
    /// <param name="include"></param>
    /// <param name="exclude"></param>
    /// <param name="separators"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static MemberRecognizeParser CreateDefault(string cross, string through, string filter, string include, string exclude, char[] separators, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var factory = new Dictionary<string, Func<IEnumerable<string>, IProjection<string>?>>(CompareConverter.ToComparer(comparison))
        {
            [nameof(Prefix)] = arguments => Prefix(arguments, comparison),
            [nameof(Suffix)] = arguments => Suffix(arguments, comparison),
            [nameof(RemovePrefix)] = arguments => RemovePrefix(arguments, comparison),
            [nameof(RemoveSuffix)] = arguments => RemoveSuffix(arguments, comparison)
        };
        return new MemberRecognizeParser(cross, through, filter, include, exclude, separators, factory, comparison);
    }
    /// <summary>
    /// 构造默认实例
    /// </summary>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static MemberRecognizeParser CreateDefault(StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        => CreateDefault("Cross:", "Through:", "Filter:", "Include:", "Exclude:", [' '], comparison);
    /// <summary>
    /// 默认实例
    /// </summary>
    public new static MemberRecognizeParser Default
        => DefaultInner.Instance;
    #endregion
    class RecognizerInner
    {
        /// <summary>
        /// 默认投影
        /// </summary>
        internal static readonly IRecognizer<string> Default = new DefaultRecognizer<string>();
    }
    class DefaultInner
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        internal static readonly MemberRecognizeParser Instance = CreateDefault();
    }
}