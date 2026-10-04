using Hand.Configuration;

namespace Hand.Rule.Configurations;

/// <summary>
/// 验证配置
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class ValidationConfiguration<TKey>
    : IConfiguration<TKey, bool>, IValidation<TKey>
{
    #region IConfiguration<TKey, bool>
    #region IConfigure<TKey, TValue>
    /// <inheritdoc />
    public abstract void Set(in TKey key, bool value);
    #endregion
    /// <inheritdoc />
    public abstract bool TryGetConfig(TKey key, out bool value);
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public abstract bool Validate(TKey argument);
    #endregion
}
