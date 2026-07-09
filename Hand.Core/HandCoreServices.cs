using Hand.Maping;
using Hand.Observer;
using System.Runtime.CompilerServices;

namespace Hand;

/// <summary>
/// 扩展方法
/// </summary>
public static class HandCoreServices
{
    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observers"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable CreateUnSubscriber<T>(this ICollection<IObserver<T>> observers, IObserver<T> observer)
        => new UnSubscriber<T>(observers, observer);
    /// <summary>
    /// 识别成员
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="recognizers"></param>
    /// <param name="members"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TMember> Recognize<TKey, TMember>(this IEnumerable<IRecognizer<TKey>> recognizers, IDictionary<TKey, TMember> members)
    {
        foreach (var recognizer in recognizers)
            members = recognizer.Recognize(members);
        return members;
    }
    ///// <summary>
    ///// 转化数组
    ///// </summary>
    ///// <typeparam name="TSource"></typeparam>
    ///// <typeparam name="TResult"></typeparam>
    ///// <param name="source"></param>
    ///// <param name="convertor"></param>
    ///// <returns></returns>
    //public static TResult[] ArrayConvert<TSource, TResult>(this  TSource[] source, Converter<TSource, TResult> convertor)
    //{
    //    var count = source.Length;
    //    var results = new TResult[count];
    //    var i = 0;
    //    foreach (var item in source)
    //    {
    //        results[i++] = convertor(item);
    //    }
    //    return results;
    //}
}