namespace Hand.States;

/// <summary>
/// 状态
/// </summary>
/// <typeparam name="TStatus"></typeparam>
public interface IState<TStatus>
{
    /// <summary>
    /// 状态
    /// </summary>
    TStatus Status { get; }
}
