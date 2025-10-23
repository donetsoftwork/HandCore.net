namespace Hand.Observer;

/// <summary>
/// 观察者通知基类(被观察者)
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="list"></param>
public class Observable<T>(ICollection<IObserver<T>> list)
    : IObservable<T>
{
    /// <summary>
    /// 观察者列表
    /// </summary>
    protected ICollection<IObserver<T>> _observers = list;
    /// <summary>
    /// 观察者通知基类(被观察者)
    /// </summary>
    public Observable()
        : this([])
    {
    }
    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="observer">观察者</param>
    /// <returns></returns>
    public virtual IDisposable Subscribe(IObserver<T> observer)
    {
        if (_observers == null || observer == null)
            return null;
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return CreateUnSubscriber(_observers, observer);
    }
    /// <summary>
    /// 构造取消订阅
    /// </summary>
    /// <param name="list"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    protected virtual IDisposable CreateUnSubscriber(ICollection<IObserver<T>> list, IObserver<T> observer)
    {
        return list.CreateUnSubscriber(observer);
    }
    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="observer">观察者</param>
    /// <param name="message">消息</param>
    protected virtual void Notify(IObserver<T> observer, T message)
    {
        if (observer == null)
            return;
        observer.OnNext(message);
    }
    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="message">消息</param>
    public virtual void Notify(T message)
    {
        if (_observers == null)
            return;
        foreach (var observer in _observers)
            Notify(observer, message);
    }
    /// <summary>
    /// 完成
    /// </summary>
    public virtual void Complete()
    {
        if (_observers == null)
            return;
        foreach (var observer in _observers)
        {
            if (observer == null)
                continue;
            observer.OnCompleted();
        }
        _observers.Clear();
    }
}
