namespace Hand.Rule.Configurations;

/// <summary>
/// 空配置
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class EmptyConfiguration<TKey>
    : ValidationConfiguration<TKey>
{
    private EmptyConfiguration() { }
    #region IConfiguration<TKey, bool>
    #region IConfigure<TKey, TValue>
    /// <inheritdoc />
    public override void Set(in TKey key, bool value) { }
    #endregion
    /// <inheritdoc />
    public override bool TryGetConfig(TKey key, out bool value)
        => value = false;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(TKey argument)
        => false;
    #endregion
    /// <summary>
    /// 默认规则
    /// </summary>
    public static readonly ValidationConfiguration<TKey> Instance = new EmptyConfiguration<TKey>();
}