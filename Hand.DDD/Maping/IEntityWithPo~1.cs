namespace Hand.Maping;

/// <summary>
/// 实体映射PO标记(用于代码生成)
/// </summary>
/// <typeparam name="TPo"></typeparam>
public interface IEntityWithPo<TPo>
{
    // 生成扩展方法 TPo ToPo(this TEntity entity);
    // 生成扩展方法 TEntity ToEntity(this TPo po);
}
