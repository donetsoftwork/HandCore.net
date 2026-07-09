namespace Hand.Configuration;

/// <summary>
/// 默认值
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IDefault<TValue>
{
    /// <summary>
    /// 默认值
    /// </summary>
    TValue DefaultValue { get; }
}
