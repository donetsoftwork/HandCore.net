namespace Hand.Rule.Configurations;

/// <summary>
/// 复合配置
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="includes"></param>
/// <param name="excludes"></param>
/// <param name="defaultValue"></param>
public class ComplexConfiguration<TKey>(ISet<TKey> includes, ISet<TKey> excludes, bool defaultValue = false)
    : ValidationConfiguration<TKey>
{
    private readonly ISet<TKey> _includes = includes;
    private readonly ISet<TKey> _excludes = excludes;
    private readonly bool _defaultValue = defaultValue;

    #region IConfiguration<TKey, bool>
    #region IConfigure<TKey, TValue>
    /// <inheritdoc />
    public override void Set(in TKey key, bool value)
    {
        if (value)
            _includes.Add(key);
        else
            _excludes.Add(key);
    }
    #endregion
    /// <inheritdoc />
    public override bool TryGetConfig(TKey key, out bool value)
    {
        if(_includes.Contains(key))
        {
            value = true;
            return true;
        }
        if (_excludes.Contains(key))
        {
            value = false;
            return true;
        }
        value = _defaultValue;
        return false;
    }
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(TKey argument)
    {
        if (_includes.Contains(argument))
            return true;
        if (_excludes.Contains(argument))
            return false;
        return _defaultValue;
    }
    #endregion
}