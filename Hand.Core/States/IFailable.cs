namespace Hand.States;

/// <summary>
/// 可失败的
/// </summary>
public interface IFailable
{
    /// <summary>
    /// 失败回调
    /// </summary>
    void OnFail();
}
