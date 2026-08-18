namespace Hand.Maping;

/// <summary>
/// 默认投影
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DefaultProjection<T> : IProjection<T>
{
    /// <inheritdoc />
    T IConverter<T, T>.Convert(T source)
        => source;
    /// <inheritdoc />
    bool IProjection<T>.TryConvert(T source, out T result)
    {
        result = source;
        return false;
    }
    /// <inheritdoc />
    IProjection<T> IProjection<T>.Reverse()
        => this;
}
