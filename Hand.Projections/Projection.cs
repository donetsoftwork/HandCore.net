using Hand.Comparers;
using Hand.Maping.Complexs;
using Hand.Naming;
using Hand.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Hand.Maping;

/// <summary>
/// 投影服务
/// </summary>
public static class Projection
{
    #region Prefix
    /// <summary>
    /// 前缀投影
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> Prefix(string prefix, StringComparison comparison = StringComparison.Ordinal)
        => new PrefixProjection(prefix, comparison);
    /// <summary>
    /// 去除前缀投影
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> RemovePrefix(string prefix, StringComparison comparison = StringComparison.Ordinal)
        => new RemovePrefixProjection(prefix, comparison);
    #endregion
    #region Suffix
    /// <summary>
    /// 后缀投影
    /// </summary>
    /// <param name="suffix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> Suffix(string suffix, StringComparison comparison = StringComparison.Ordinal)
        => new SuffixProjection(suffix, comparison);
    /// <summary>
    /// 去除后缀投影
    /// </summary>
    /// <param name="suffix"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> RemoveSuffix(string suffix, StringComparison comparison = StringComparison.Ordinal)
        => new RemoveSuffixProjection(suffix, comparison);
    #endregion
    #region Trim
    /// <summary>
    /// 字符去除投影
    /// </summary>
    /// <param name="trimChars"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> Trim(params char[] trimChars)
        => new TrimProjection(trimChars);
    /// <summary>
    /// 前导字符去除投影
    /// </summary>
    /// <param name="starts"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> TrimStart(params char[] starts)
        => new TrimStartProjection(starts);
    /// <summary>
    /// 结尾字符去除投影
    /// </summary>
    /// <param name="ends"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> TrimEnd(params char[] ends)
        => new TrimEndProjection(ends);
    #endregion
    #region Replace
    /// <summary>
    /// 替换投影
    /// </summary>
    /// <param name="fragment"></param>
    /// <param name="replacement"></param>
    /// <param name="start"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> Replace(string fragment, string replacement, int start = 0, StringComparison comparison = StringComparison.Ordinal)
        => new ReplaceProjection(fragment, replacement, start, comparison);
    /// <summary>
    /// 替换前缀投影
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="replacement"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> ReplacePrefix(string prefix, string replacement, StringComparison comparison = StringComparison.Ordinal)
        => new ReplacePrefixProjection(prefix, replacement, comparison);
    /// <summary>
    /// 替换后缀投影
    /// </summary>
    /// <param name="suffix"></param>
    /// <param name="replacement"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> ReplaceSuffix(string suffix, string replacement, StringComparison comparison = StringComparison.Ordinal)
        => new ReplaceSuffixProjection(suffix, replacement, comparison);
    #endregion
    #region Verify
    /// <summary>
    /// 校验投影
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<TArgument> Verify<TArgument>(IValidation<TArgument> validation)
        => new VerifyProjection<TArgument>(validation);
    /// <summary>
    /// 校验投影
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="validation"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<TArgument> ToProjection<TArgument>(this IValidation<TArgument> validation)
        => new VerifyProjection<TArgument>(validation);
    #endregion
    #region Naming
    /// <summary>
    /// 命名规则投影
    /// </summary>
    /// <param name="validation"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> Naming(IValidation<string> validation, INameConverter converter)
        => new NamingProjection(validation, converter);
    /// <summary>
    /// 命名规则投影
    /// </summary>
    /// <param name="converter"></param>
    /// <param name="validation"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<string> ToProjection(this INameConverter converter, IValidation<string> validation)
        => new NamingProjection(validation, converter);
    #endregion
    #region EachIn
    /// <summary>
    /// 逐个投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="failContinue"></param>
    /// <param name="projections"></param>
    /// <returns></returns>
    public static IProjection<T> EachIn<T>(bool failContinue, params IProjection<T>[] projections)
    {
        return projections.Length switch
        {
            0 => Default<T>(),
            1 => projections[0],
            _ => new EachInProjection<T>(failContinue, projections),
        };
    }
    /// <summary>
    /// 逐个投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projections"></param>
    /// <param name="failContinue"></param>
    /// <returns></returns>
    public static IProjection<T> ToEachIn<T>(this IEnumerable<IProjection<T>> projections, bool failContinue = true)
        => EachIn(failContinue, projections.ToArray());
    /// <summary>
    /// 逐个投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projections"></param>
    /// <param name="failContinue"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<T> ToEachIn<T>(this IProjection<T>[] projections, bool failContinue = true)
        => EachIn(failContinue, projections);
    #endregion
    #region FirstReturn
    /// <summary>
    /// 快速结束投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projections"></param>
    /// <returns></returns>
    public static IProjection<T> FirstReturn<T>(params IProjection<T>[] projections)
    {
        return projections.Length switch
        {
            0 => Default<T>(),
            1 => projections[0],
            _ => new FirstReturnProjection<T>(projections),
        };
    }
    /// <summary>
    /// 快速结束投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projections"></param>
    /// <returns></returns>
    public static IProjection<T> ToFirstReturn<T>(this IEnumerable<IProjection<T>> projections)
        => FirstReturn(projections.ToArray());
    /// <summary>
    /// 快速结束投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="projections"></param>
    /// <returns></returns>
    public static IProjection<T> ToFirstReturn<T>(this IProjection<T>[] projections)
        => FirstReturn(projections);
    #endregion
    /// <summary>
    /// 默认投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IProjection<T> Default<T>()
        => new DefaultProjection<T>();
    #region Filter
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        foreach (var projection in projections)
            source = Filter(projection, source, comparer);
        return source;
    }
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Filter(projections, source, CompareConverter.GetComparer(source));
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        int count = source.Count;
        if (count == 0)
            return source;
        var result = new Dictionary<TKey, TValue>(count, comparer);
        TryConvert(projection, source, result);
        return result;
    }
    /// <summary>
    /// 过滤投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Filter<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Filter(projection, source, CompareConverter.GetComparer(source));
    #endregion
    #region Through
    /// <summary>
    /// 穿透投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Through<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        foreach (var projection in projections)
            source = Through(projection, source, comparer);
        return source;
    }
    /// <summary>
    /// 穿透投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Through<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Through(projections, source, CompareConverter.GetComparer(source));
    /// <summary>
    /// 穿透投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Through<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        int count = source.Count;
        if (count == 0)
            return source;
        var result = new Dictionary<TKey, TValue>(count, comparer);
        foreach (var key in source.Keys)
            Convert(projection, source, result, key);

        return result;
    }
    /// <summary>
    /// 穿透投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Through<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Through(projection, source, CompareConverter.GetComparer(source));
    #endregion
    #region Cross
    /// <summary>
    /// 交叉投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Cross<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        foreach (var projection in projections)
            source = Cross(projection, source, comparer);
        return source;
    }
    /// <summary>
    /// 交叉投影
    /// </summary>
    /// <param name="projections"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Cross<TKey, TValue>(this IEnumerable<IProjection<TKey>> projections, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Cross(projections, source, CompareConverter.GetComparer(source));
    /// <summary>
    /// 交叉投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Cross<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        int count = source.Count;
        if (count == 0)
            return source;
        var result = new Dictionary<TKey, TValue>(source, comparer);
        TryConvert(projection, source, result);
        return result;
    }
    /// <summary>
    /// 交叉投影
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Cross<TKey, TValue>(this IProjection<TKey> projection, IDictionary<TKey, TValue> source)
        where TKey : notnull
        => Cross(projection, source, CompareConverter.GetComparer(source));
    #endregion
    /// <summary>
    /// 尝试转化
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="projection"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    private static void TryConvert<TKey, TValue>(IProjection<TKey> projection, IDictionary<TKey, TValue> from, Dictionary<TKey, TValue> to)
        where TKey : notnull
    {
        foreach (var key in from.Keys)
            TryConvert(projection, from, to, key);
    }
    /// <summary>
    /// 尝试转化
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="projection"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static bool TryConvert<TKey, TValue>(IProjection<TKey> projection, IDictionary<TKey, TValue> from, Dictionary<TKey, TValue> to, TKey key)
        where TKey : notnull
    {
        if (projection.TryConvert(key, out var converted))
        {
            if (to.ContainsKey(converted))
                return false;
            to[converted] = from[key];
            return true;
        }
        return false;
    }
    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="projection"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="key"></param>
    private static void Convert<TKey, TValue>(IProjection<TKey> projection, IDictionary<TKey, TValue> from, Dictionary<TKey, TValue> to, TKey key)
        where TKey : notnull
    {
        if (TryConvert(projection, from, to, key))
            return;
        to[key] = from[key];
    }
}
