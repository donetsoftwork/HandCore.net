using Hand.Job;

namespace Hand.States;

/// <summary>
/// 结果回调
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class ResultCallBack<TResult>
    : StateCallBack, IJobResult<TResult>
{
    #region 配置
    private TResult _result = default;
    /// <summary>
    /// 结果
    /// </summary>
    public TResult Result
        => _result;
    #endregion
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="result"></param>
    public void OnSuccess(TResult result)
    {
        _result = result;
        OnSuccess();
    }
}
