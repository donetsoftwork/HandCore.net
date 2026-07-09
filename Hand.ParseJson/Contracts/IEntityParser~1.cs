namespace Hand.ParseJson.Contracts;

/// <summary>
/// 实体解析器
/// </summary>
public interface IEntityParser<TEntity>
    : IEntityParser, IJsonParser<TEntity>
{
}
