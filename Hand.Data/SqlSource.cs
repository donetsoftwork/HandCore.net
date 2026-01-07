using System.Data;
using System.Data.Common;
using Hand.Disposables;
#if !NET7_0_OR_GREATER
using DbDataSource = Hand.Data.StringDataSource;
#endif

namespace Hand.Data;

/// <summary>
/// SQL数据源
/// </summary>
/// <param name="dataSource"></param>
/// <param name="commandTimeout"></param>
public class SqlSource(DbDataSource dataSource, int? commandTimeout = null)
    : ISqlSource
{
    #region 配置
    private readonly DbDataSource _dataSource = dataSource;
    private readonly int? _commandTimeout = commandTimeout;
    /// <summary>
    /// 数据源
    /// </summary>
    public DbDataSource DataSource
        => _dataSource;
    /// <inheritdoc/>
    public int? CommandTimeout
        => _commandTimeout;
    /// <inheritdoc/>
    IDbTransaction ISqlSource.Transaction
        => null;
    #endregion
    /// <inheritdoc/>
    public DbConnection CreateConnection()
        => _dataSource.CreateConnection();
    /// <inheritdoc/>
    public IDisposableResource<DbCommand> CreateCommand(string commandText)
    {
        var connection = _dataSource.CreateConnection();
        var command = connection.CreateCommand();
        if (_commandTimeout.HasValue)
            command.CommandTimeout = _commandTimeout.Value;
        command.CommandText = commandText;
        command.CommandType = CommandType.Text;

        var commandWrapped = new DisposableResource<DbCommand>(command);
        // 将连接也加入到可释放资源中，以便统一管理
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        commandWrapped.AddAsync(connection);
#else
        commandWrapped.Add(connection);
#endif        
        return commandWrapped;
    }
    #region BeginTransaction
    /// <summary>
    /// 开启事务
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<TransactionSource> BeginTransaction(int? commandTimeout = null, CancellationToken token = default)
    {
        var connection = _dataSource.CreateConnection();
        await connection.OpenAsync(token);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(token);
#else
        var transaction = connection.BeginTransaction();
#endif
        return new TransactionSource(connection, transaction, commandTimeout ?? _commandTimeout);
    }
    /// <summary>
    /// 开启事务
    /// </summary>
    /// <param name="level"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<TransactionSource> BeginTransaction(IsolationLevel level, int? commandTimeout = null, CancellationToken token = default)
    {
        var connection = _dataSource.CreateConnection();
        await connection.OpenAsync(token);
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(level, token);
#else
        var transaction = connection.BeginTransaction(level);
#endif
        return new TransactionSource(connection, transaction, commandTimeout ?? _commandTimeout);
    }
    #endregion
}
