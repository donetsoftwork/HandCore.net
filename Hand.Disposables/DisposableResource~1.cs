using Hand.Structural;
using System;

namespace Hand.Disposables;

/// <summary>
/// 可释放资源包装器
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class DisposableResource<TResource>
    : CompositeDisposable
    , IDisposableResource<TResource>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    where TResource : IAsyncDisposable
#else
    where TResource : IDisposable
#endif    
{
    #region 配置
    private readonly TResource _original;
    /// <inheritdoc/>
    public TResource Original 
        => _original;
    #endregion
    /// <summary>
    /// 可释放资源包装器
    /// </summary>
    /// <param name="original"></param>
    public DisposableResource(TResource original)
    {
        _original = original;
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        AddAsync(original);
#else
        Add(original);
#endif
    }
}
