using Hand.Disposables;
using System.Data;
using System.Data.Common;

namespace Hand.Data;

/// <summary>
/// SQL数据源
/// </summary>
public interface ISqlSource
{
    /// <summary>
    /// 构造Command
    /// </summary>
    /// <param name="commandText"></param>
    /// <returns></returns>
    IDisposableResource<DbCommand> CreateCommand(string commandText);
    /// <summary>
    /// 创建连接
    /// </summary>
    /// <returns></returns>
    DbConnection CreateConnection();
    /// <summary>
    /// 超时设置
    /// </summary>
    int? CommandTimeout { get; }
    /// <summary>
    /// 事务
    /// </summary>
    IDbTransaction Transaction { get; }
    /// <summary>
    /// 单结果
    /// </summary>
    CommandBehavior SingleBehavior { get; }
    /// <summary>
    /// 单行
    /// </summary>
    CommandBehavior RowBehavior { get; }
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="command"></param>
    /// <param name="behavior"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<DbDataReader> ExecuteReaderAsync(DbCommand command, CommandBehavior behavior, CancellationToken token);
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="command"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<int> ExecuteNonQueryAsync(DbCommand command, CancellationToken token);
}
