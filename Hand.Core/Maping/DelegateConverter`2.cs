namespace Hand.Maping;

/// <summary>
/// 委托转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="original"></param>
public class DelegateConverter<TSource, TDest>(Converter<TSource, TDest> original)
    : IConverter<TSource, TDest>
{
    private readonly Converter<TSource, TDest> _original = original;

    /// <inheritdoc />
    public TDest Convert(TSource source)
        => _original(source);
}
