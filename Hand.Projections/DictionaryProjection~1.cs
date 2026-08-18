using Hand.Comparers;
using System.Collections.Generic;

namespace Hand.Maping;

/// <summary>
/// 字典映射投影
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="provider"></param>
public class DictionaryProjection<TKey>(IDictionary<TKey, TKey> provider)
    : IProjection<TKey>
     where TKey : notnull
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
    /// <inheritdoc />
    IProjection<TKey> IProjection<TKey>.Reverse()
    {
        var provider = new Dictionary<TKey, TKey>(_provider.Count, CompareConverter.GetComparer(_provider));
        foreach (var item in _provider)
            provider[item.Value] = item.Key;
        return new DictionaryProjection<TKey>(provider);
    }
}
