using Hand.Observer;
using System.Collections.Concurrent;

namespace Hand.Dataflow;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <param name="inputs"></param>
/// <param name="action"></param>
public class ActionJob<TInput>(IProducerConsumerCollection<TInput> inputs, Action<TInput> action)
{
    #region 配置
    private readonly IProducerConsumerCollection<TInput> _inputs = inputs;
    private readonly Action<TInput> _action = action;
    private readonly Producer<TInput> _producer = new(inputs);
    private readonly Observable<TInput> _consumer = new();
    private readonly TaskCompletionSource<object> _completion = new();
    /// <summary>
    /// 生产者
    /// </summary>
    public Producer<TInput> Producer 
        => _producer;
    /// <summary>
    /// 消费者
    /// </summary>
    public Observable<TInput> Consumer 
        => _consumer;
    #endregion
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="input"></param>
    public void Execute(TInput input)
    {
        _action(input);
        _consumer.Notify(input);
    }
}
