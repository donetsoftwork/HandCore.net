namespace Hand.Observer;

/// <summary>
/// 观察者基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ObserverBase<T> : IObserver<T>
{
    #region 配置
    /// <summary>
    /// 取消订阅对象
    /// </summary>
    protected IDisposable _unsubscriber;
    #endregion
    #region IObserver<T>
    void IObserver<T>.OnCompleted()
        => UnSubscribe();
    void IObserver<T>.OnError(Exception error)
        => OnError(error);
    void IObserver<T>.OnNext(T value)
        => OnNext(value);
    #endregion
    /// <summary>
    /// 完成
    /// </summary>
    protected virtual void OnCompleted()
        => UnSubscribe();
    /// <summary>
    /// 出错
    /// </summary>
    /// <param name="error"></param>
    protected virtual void OnError(Exception error)
    {
    }
    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="value"></param>
    protected virtual void OnNext(T value)
    {
        Receive(value);
    }
    /// <summary>
    /// 接受新的通知(处理通知逻辑)
    /// </summary>
    /// <param name="value">通知对象</param>
    protected abstract void Receive(T value);
    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public virtual bool Subscribe(IObservable<T> provider)
    {
        if (provider == null)
            return false;
        _unsubscriber = provider.Subscribe(this);
        return true;
    }
    /// <summary>
    /// 取消订阅
    /// </summary>
    public virtual void UnSubscribe()
        => _unsubscriber?.Dispose();
}
