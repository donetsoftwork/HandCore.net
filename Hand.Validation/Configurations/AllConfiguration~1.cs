namespace Hand.Rule.Configurations;

/// <summary>
/// 全真配置
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class AllConfiguration<TKey>
    : ValidationConfiguration<TKey>
{
    private AllConfiguration() { }
    #region IConfiguration<TKey, bool>
    #region IConfigure<TKey, TValue>
    /// <inheritdoc />
    public override void Set(in TKey key, bool value) { }
    #endregion
    /// <inheritdoc />
    public override bool TryGetConfig(TKey key, out bool value)
    {
        value = true;
        return false;
    }
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(TKey argument)
        => true;
    #endregion
    /// <summary>
    /// 默认规则
    /// </summary>
    public static readonly ValidationConfiguration<TKey> Instance = new AllConfiguration<TKey>();
}
