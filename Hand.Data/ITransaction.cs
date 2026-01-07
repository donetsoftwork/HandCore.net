using System;
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Hand.Data;

/// <summary>
/// 事务接口
/// </summary>
public interface ITransaction
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    : IAsyncDisposable
#else
    : IDisposable
#endif
{
    /// <summary>
    /// 提交事务
    /// </summary>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    ValueTask CommitAsync(CancellationToken cancellationToken = default);
#else
    void Commit();
#endif
    /// <summary>
    /// 回滚事务
    /// </summary>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    ValueTask RollbackAsync(CancellationToken cancellationToken = default);
#else
    void Rollback();
#endif
}
