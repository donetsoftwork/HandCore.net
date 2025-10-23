namespace Hand.Observer;

/// <summary>
/// 订阅服务
/// </summary>
public static class SubscribeServices
{
    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observers"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    public static IDisposable CreateUnSubscriber<T>(this ICollection<IObserver<T>> observers, IObserver<T> observer)
    {
        return new UnSubscriber<T>(observers, observer);
    }
}