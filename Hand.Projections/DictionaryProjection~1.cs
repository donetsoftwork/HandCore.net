using System.Collections.Generic;

namespace Hand.Maping;

/// <summary>
/// 字典投影
/// </summary>
/// <typeparam name="TTey"></typeparam>
/// <param name="provider"></param>
public class DictionaryProjection<TTey>(IDictionary<TTey, TTey> provider)
    : IProjection<TTey>
{
    private readonly IDictionary<TTey, TTey> _provider = provider;
    #region IProjection<TTey, TValue>
    /// <inheritdoc />
    public bool TryConvert(TTey source, out TTey result)
        => _provider.TryGetValue(source, out result!);
    #endregion
}
