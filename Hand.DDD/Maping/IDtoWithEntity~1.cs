namespace Hand.Maping;

/// <summary>
/// DTO映射实体标记(用于代码生成)
/// </summary>
public interface IDtoWithEntity<TEntity>
{
    // 生成扩展方法 TEntity ToEntity(this Dto dto);
    // 生成扩展方法 TDto ToDto(this TEntity entity);
}
