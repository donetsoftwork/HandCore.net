namespace Hand.Rule.Configurations;

/// <summary>
/// 包含配置
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="original"></param>
public class IncludeConfiguration<TKey>(ISet<TKey> original)
    : ValidationConfiguration<TKey>
{
    private readonly ISet<TKey> _original = original;

    #region IConfiguration<TKey, bool>
    #region IConfigure<TKey, TValue>
    /// <inheritdoc />
    public override void Set(in TKey key, bool value)
        => _original.Add(key);
    #endregion
    /// <inheritdoc />
    public override bool TryGetConfig(TKey key, out bool value)
        => value = _original.Contains(key);
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(TKey argument)
        => _original.Contains(argument);
    #endregion
}
