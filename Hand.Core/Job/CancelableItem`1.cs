using Hand.Structural;

namespace Hand.Job;

/// <summary>
/// 可取消元素
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <param name="original"></param>
/// <param name="token"></param>
public readonly ref struct CancelableItem<TItem>(TItem original, CancellationToken token)
    : IWrapper<TItem>
{
    #region 配置
    private readonly TItem _original = original;
    private readonly CancellationToken _token = token;
    /// <inheritdoc />
    public TItem Original 
        => _original;
    /// <summary>
    /// CancellationToken
    /// </summary>
    public CancellationToken Token
        => _token;
    #endregion
}
