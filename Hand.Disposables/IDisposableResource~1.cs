using Hand.Structural;
using System;

namespace Hand.Disposables;

/// <summary>
/// 可释放资源接口
/// </summary>
/// <typeparam name="TResource"></typeparam>
public interface IDisposableResource<TResource>
    : IWrapper<TResource>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncDisposable
    where TResource : IAsyncDisposable
#else
    , IDisposable
    where TResource : IDisposable
#endif    
{
}
