using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Hand.Disposables;

/// <summary>
/// 组合释放器
/// </summary>
public class CompositeDisposable
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    : IAsyncDisposable
#else
    : IDisposable
#endif
{
    #region 配置
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private readonly List<IAsyncDisposable> _asyncDisposables = [];
    #endif
    private readonly List<IDisposable> _disposables = [];
    #endregion

    /// <summary>
    /// 添加待释放项
    /// </summary>
    /// <param name="item"></param>
    public void Add(IDisposable item)
    {
        _disposables.Add(item);
    }
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 添加异步待释放项
    /// </summary>
    /// <param name="asyncDisposable"></param>
    public void AddAsync(IAsyncDisposable asyncDisposable)
    {
        _asyncDisposables.Add(asyncDisposable);
    }
    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _asyncDisposables)
            await disposable.DisposeAsync();
        DisposeCore();
    }
#else
    /// <inheritdoc/>
    public void Dispose()
        => DisposeCore();
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisposeCore()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();
    }
}
