namespace Hand.Creational;

/// <summary>
/// 默认构造器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class DefaultCreater<TEntity>
     : ICreator<TEntity>
    where TEntity : new()
{
    /// <inheritdoc />
    public TEntity Create()
        => new();
}
