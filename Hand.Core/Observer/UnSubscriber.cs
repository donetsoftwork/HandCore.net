namespace Hand.Observer;

/// <summary>
/// 取消订阅
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="observers"></param>
/// <param name="observer"></param>
public class UnSubscriber<T>(ICollection<IObserver<T>> observers, IObserver<T> observer)
    : IDisposable
{
    private readonly ICollection<IObserver<T>> _observers = observers;
    private readonly IObserver<T> _observer = observer ?? throw new ArgumentException(nameof(observer));

    /// <summary>
    /// 通过IDisposable(模式)取消订阅
    /// </summary>
    void IDisposable.Dispose()
        => UnSubscribe();
    /// <summary>
    /// 取消订阅
    /// </summary>
    protected virtual void UnSubscribe()
    {
        if (_observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}
