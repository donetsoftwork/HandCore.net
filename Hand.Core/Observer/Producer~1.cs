using System.Collections.Concurrent;

namespace Hand.Observer;

/// <summary>
/// 生成者
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="collection"></param>
public class Producer<T>(IProducerConsumerCollection<T> collection)
    : ObserverBase<T>
{
    #region 配置
    private readonly IProducerConsumerCollection<T> _collection = collection;
    #endregion
    /// <inheritdoc />
    protected override void Receive(T item)
        => _collection.TryAdd(item);
}
