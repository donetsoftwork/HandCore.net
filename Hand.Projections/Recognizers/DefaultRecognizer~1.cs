using System.Collections.Generic;

namespace Hand.Maping.Recognizers;

/// <summary>
/// 默认识别器
/// </summary>
/// <typeparam name="TKey"></typeparam>
public sealed class DefaultRecognizer<TKey>
    : IRecognizer<TKey>
    where TKey : notnull
{
    /// <inheritdoc />
    IDictionary<TKey, TValue> IRecognizer<TKey>.Recognize<TValue>(IDictionary<TKey, TValue> source)
        => source;
}
