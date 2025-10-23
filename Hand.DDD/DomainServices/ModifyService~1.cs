using Hand.Deltas;
using Hand.Models;
using Hand.Storage;

namespace Hand.DomainServices;

/// <summary>
/// 修改服务
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="repository"></param>
public class ModifyService<TEntity>(IRepository<TEntity> repository)
     where TEntity : IEntity<long>
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public  async Task SaveAsync(IDelta<TEntity> delta)
    {
        await repository.UpdateAsync(delta.Instance, delta.Data.Keys);
    }
}
