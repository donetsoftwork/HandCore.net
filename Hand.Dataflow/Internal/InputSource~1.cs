using System.Collections.Concurrent;

namespace Hand.Dataflow.Internal;

/// <summary>
/// 输入源
/// </summary>
/// <typeparam name="TInput"></typeparam>
public class InputSource<TInput>
{
    private readonly ConcurrentBag<TInput> _inputs = [];

    /// <summary>
    /// 输入
    /// </summary>
    /// <param name="input"></param>
    public void Input(TInput input)
        => _inputs.Add(input);
}
