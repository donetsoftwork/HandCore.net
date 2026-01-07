using System;
using System.Data.Common;

namespace Hand.Data;

/// <summary>
/// 数据源
/// </summary>
public sealed class StringDataSource(string connectionString, Func<string, DbConnection> factoryFunc)
#if NET7_0_OR_GREATER
    : DbDataSource
#endif
{
    #region 配置
    private readonly string _connectionString = connectionString;
    private readonly Func<string, DbConnection> _factoryFunc = factoryFunc;
    /// <summary>
    /// 连接字符串
    /// </summary>
#if NET7_0_OR_GREATER
    public override string ConnectionString
#else
    public string ConnectionString
#endif
        => _connectionString;
    /// <summary>
    /// 工厂方法
    /// </summary>
    public Func<string, DbConnection> FactoryFunc
        => _factoryFunc;
    #endregion
    /// <summary>
    /// 构造数据库连接
    /// </summary>
    /// <returns></returns>
#if NET7_0_OR_GREATER
    protected override DbConnection CreateDbConnection()
#else
    public DbConnection CreateConnection()
#endif
        => _factoryFunc(_connectionString);
}
