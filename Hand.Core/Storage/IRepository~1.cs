using Hand.Models;

namespace Hand.Storage;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity>
    : IAsyncGet<long, TEntity>
    where TEntity : IEntity<long>
{
    /// <summary>
    /// 插入
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity, CancellationToken token);
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <param name="fieldNames"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, CancellationToken token, params IEnumerable<string> fieldNames);
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entity, CancellationToken token);
}
