namespace Hand.Rule;

/// <summary>
/// 满足任一规则(相当于OR)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="rules"></param>
public class ValidateAny<TEntity>(params IEnumerable<IValidation<TEntity>> rules)
    : IValidation<TEntity>
{
    private readonly IEnumerable<IValidation<TEntity>> _rules = rules;
    /// <summary>
    /// 规则
    /// </summary>
    public IEnumerable<IValidation<TEntity>> Rules
        => _rules;
    /// <inheritdoc />
    public bool Validate(TEntity entity)
        => _rules.Any(x => x.Validate(entity));
}