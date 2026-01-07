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
}
