using Hand.Disposables;
using System.Data;
using System.Data.Common;
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
using System.Threading;
#else
using System;
#endif

namespace Hand.Data;

/// <summary>
/// 事务数据源
/// </summary>
/// <param name="connection"></param>
/// <param name="transaction"></param>
/// <param name="commandTimeout"></param>
public class TransactionSource(DbConnection connection, DbTransaction transaction, int? commandTimeout = null)
    : ISqlSource
    , ITransaction
{
    #region 配置
    private readonly DbConnection _connection = connection;
    private readonly DbTransaction _transaction = transaction;
    private readonly int? _commandTimeout = commandTimeout;
    private bool _rolledBack = true;
    /// <summary>
    /// 数据源
    /// </summary>
    public DbConnection Connection
        => _connection;
    /// <inheritdoc/>
    public IDbTransaction Transaction
        => _transaction;
    /// <summary>
    /// 超时设置
    /// </summary>
    public int? CommandTimeout
        => _commandTimeout;
    #endregion
    /// <inheritdoc/>
    DbConnection ISqlSource.CreateConnection()
        => _connection;
    /// <inheritdoc/>
    public IDisposableResource<DbCommand> CreateCommand(string commandText)
    {
        var command = _connection.CreateCommand();
        command.Transaction = _transaction;
        if (_commandTimeout.HasValue)
            command.CommandTimeout = _commandTimeout.Value;
        command.CommandText = commandText;
        command.CommandType = CommandType.Text;
        return new DisposableResource<DbCommand>(command);
    }
    #region ITransaction
    /// <summary>
    /// 提交事务
    /// </summary>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public async ValueTask CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
        _rolledBack = false;
    }
#else
    public void Commit()
    {
        _transaction.Commit();
        _rolledBack = false;
    }
#endif
    /// <summary>
    /// 回滚事务
    /// </summary>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public async ValueTask RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
        _rolledBack = false;
    }
#else
    public void Rollback()
    {
        _transaction.Rollback();
        _rolledBack = false;
    }
#endif
    /// <inheritdoc/>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public async ValueTask DisposeAsync()
    {
        if (_rolledBack) 
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            catch { }
        }
        try
        {
            await _transaction.DisposeAsync();
        }
        catch { }
        try
        {
            await _connection.DisposeAsync();
        }
        catch { }
    }
#else
    public void Dispose()
    {
         if (_rolledBack) 
        {
            try
            {
                _transaction.Rollback();
            }
            catch { }
        }
        try
        {
            _transaction.Dispose();
        }
        catch { }
        try
        {
            _connection.Dispose();
        }
        catch { }
    }
#endif
    #endregion
}
