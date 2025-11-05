namespace Hand.States;

/// <summary>
/// 可处理异常的
/// </summary>
public interface IExceptionable
{
    /// <summary>
    /// 触发异常回调
    /// </summary>
    /// <param name="exception"></param>
    void OnException(Exception exception);
}
