namespace Hand.Rule;

/// <summary>
/// 验证规则
/// </summary>
public interface IValidation<TEntity>
{
    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Validate(TEntity entity);
}
