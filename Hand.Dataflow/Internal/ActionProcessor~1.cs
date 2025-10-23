using Hand.Job;

namespace Hand.Dataflow.Internal;

/// <summary>
/// 操作处理器
/// </summary>
/// <typeparam name="TData"></typeparam>
public class ActionProcessor<TData>
    : IProcessor
{
    #region IProcessor
    public bool Run()
    {
        throw new NotImplementedException();
    }
    #endregion
}
