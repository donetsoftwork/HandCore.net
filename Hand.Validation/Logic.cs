using Hand.Comparers;
using Hand.Rule.Logics;
using System;
#if !NET7_0
using System.Collections.Frozen;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Hand.Rule;

/// <summary>
/// 逻辑服务
/// </summary>
public static class Logic
{    
    #region And
    /// <summary>
    /// 与逻辑
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="and"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IValidation<TArgument> And<TArgument>(this AndLogic<TArgument> and, IValidation<TArgument> other)
    {
        if (other == TrueLogic<TArgument>.Instance)
            return and;
        if (other ==  FalseLogic<TArgument>.Instance)
            return other;
        if (other is AndLogic<TArgument> otherAnd)
        {
            and.AddRange(otherAnd.Items);
        }
        else if (other is OrLogic<TArgument> otherOr)
        {
            switch (otherOr.ItemCount)
            {
                case 0:
                    return FalseLogic<TArgument>.Instance;
                case 1:
                    and.Add(otherOr.Items.First());
                    break;
                default:
                    and.Add(other);
                    break;
            }
        }
        else
        {
            and.Add(other);
        }
        return and;
    }
    /// <summary>
    /// 与逻辑
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IValidation<TArgument> And<TArgument>(this IValidation<TArgument> validation, IValidation<TArgument> other)
    {
        if (validation == TrueLogic<TArgument>.Instance)
            return other;
        if (validation ==  FalseLogic<TArgument>.Instance)
            return validation;
        if(validation is AndLogic<TArgument> and)
            return And(and, other);
        if (other == TrueLogic<TArgument>.Instance)
            return validation;
        if (other ==  FalseLogic<TArgument>.Instance)
            return other;
        if (other is AndLogic<TArgument> otherAnd)
            return And(otherAnd, validation);
        return And(new AndLogic<TArgument>([validation]), other);
    }
    #endregion
    #region Or
    /// <summary>
    /// 与逻辑
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="or"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IValidation<TArgument> Or<TArgument>(this OrLogic<TArgument> or, IValidation<TArgument> other)
    {
        if (other == TrueLogic<TArgument>.Instance)
            return or;
        if (other ==  FalseLogic<TArgument>.Instance)
            return or;
        if (other is OrLogic<TArgument> otherOr)
        {
            or.AddRange(otherOr.Items);
        }
        else if (other is AndLogic<TArgument> otherAnd)
        {
            switch (otherAnd.ItemCount)
            {
                case 0:
                    return or;
                case 1:
                    or.Add(otherAnd.Items.First());
                    break;
                default:
                    or.Add(other);
                    break;
            }
        }
        else
        {
            or.Add(other);
        }
        return or;
    }
    /// <summary>
    /// 与逻辑
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IValidation<TArgument> Or<TArgument>(this IValidation<TArgument> validation, IValidation<TArgument> other)
    {
        if (validation == TrueLogic<TArgument>.Instance)
            return validation;
        if (validation == FalseLogic<TArgument>.Instance)
            return other;
        if (validation is OrLogic<TArgument> or)
            return Or(or, other);
        if (other == TrueLogic<TArgument>.Instance)
            return other;
        if (other == FalseLogic<TArgument>.Instance)
            return validation;
        if (other is OrLogic<TArgument> otherOr)
            return Or(otherOr, validation);
        return Or(new OrLogic<TArgument>([validation]), other);
    }
    #endregion
    #region Not
    /// <summary>
    /// 非逻辑
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <returns></returns>
    public static IValidation<TArgument> Not<TArgument>(this IValidation<TArgument> validation)
    {
        // 负负得正
        if (validation is NotLogic<TArgument> not)
            return not.Checker;
        if (validation == FalseLogic<TArgument>.Instance)
            return TrueLogic<TArgument>.Instance;
        if (validation == TrueLogic<TArgument>.Instance)
            return FalseLogic<TArgument>.Instance;
        return new NotLogic<TArgument>(validation);
    }
    /// <summary>
    /// 否定
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Not<TArgument>(this IValidation<TArgument> validation, TArgument argument)
        => !validation.Validate(argument);
    #endregion
    /// <summary>
    /// 恒真逻辑
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<TArgument> True<TArgument>()
        => TrueLogic<TArgument>.Instance;
    /// <summary>
    /// 恒假逻辑
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<TArgument> False<TArgument>()
        => FalseLogic<TArgument>.Instance;
    #region Prefix
    /// <summary>
    /// 前缀逻辑
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<string> Prefix(string prefix, StringComparison comparison = StringComparison.Ordinal)
        => new PrefixRule(prefix, comparison);
    #endregion
    #region Suffix
    /// <summary>
    /// 后缀逻辑
    /// </summary>
    /// <param name="suffix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<string> Suffix(string suffix, StringComparison comparison = StringComparison.Ordinal)
        => new SuffixRule(suffix, comparison);
    #endregion
    #region Include
    /// <summary>
    /// 包含逻辑
    /// </summary>
    /// <param name="fragment"></param>
    /// <param name="start"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<string> Include(string fragment, int start = 0, StringComparison comparison = StringComparison.Ordinal)
        => new IncludeRule(fragment, start, comparison);
    /// <summary>
    /// 包含任一字符逻辑
    /// </summary>
    /// <param name="start"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<string> IncludeAny(int start, params char[] parts)
        => new IncludeAnyRule(start, parts);
    /// <summary>
    /// 包含任一字符逻辑
    /// </summary>
    /// <param name="parts"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<string> IncludeAny(params char[] parts)
        => new IncludeAnyRule(0, parts);
    #endregion
    #region Included
    /// <summary>
    /// 被包含验证规则(成员之一)
    /// </summary>
    /// <param name="members"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<TMember> Included<TMember>(ISet<TMember> members)
        => new IncludedRule<TMember>(members);
    /// <summary>
    /// 被包含验证规则(成员之一)
    /// </summary>
    /// <param name="comparer"></param>
    /// <param name="members"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<TMember> Included<TMember>(IEqualityComparer<TMember> comparer, params IEnumerable<TMember> members)
#if NET7_0
        => new IncludedRule<TMember>(members.ToHashSet(comparer));
#else
        => new IncludedRule<TMember>(members.ToFrozenSet(comparer));
#endif
    /// <summary>
    /// 被包含验证规则(成员之一)
    /// </summary>
    /// <param name="members"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IValidation<TMember> Included<TMember>(params IEnumerable<TMember> members)
        => Included(EqualityComparer<TMember>.Default, members);
    #endregion
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="validation"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IValidation<TKey> validation, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        int count = source.Count;
        if (count == 0)
            return source;
        var result = new Dictionary<TKey, TValue>(count, comparer);
        foreach (var key in source.Keys)
        {
            if (validation.Validate(key))
                result[key] = source[key];
        }
        return result;
    }
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="validation"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IValidation<TKey> validation, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Filter(validation, source, CompareConverter.GetComparer(source));
}
