namespace Hand.States;

/// <summary>
/// 可取消的
/// </summary>
public interface ICancelable
{
    /// <summary>
    /// 取消
    /// </summary>
    void OnCancel();
}
