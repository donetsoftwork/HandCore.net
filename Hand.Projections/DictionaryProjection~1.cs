using System.Collections.Generic;

namespace Hand.Maping;

/// <summary>
/// 字典投影
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="provider"></param>
public class DictionaryProjection<TKey>(IDictionary<TKey, TKey> provider)
    : IProjection<TKey>
{
    private readonly IDictionary<TKey, TKey> _provider = provider;
    #region IProjection<TTey, TValue>
    /// <inheritdoc />
    public bool TryConvert(TKey source, out TKey result)
        => _provider.TryGetValue(source, out result!);
    #endregion
    /// <inheritdoc />
    TKey IConverter<TKey, TKey>.Convert(TKey source)
    {
        if (_provider.TryGetValue(source, out var result))
            return result;
        return source;
    }
}
