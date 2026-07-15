using System.Text.Json;

namespace Hand.ParseJson.Contracts;

/// <summary>
/// Json解析字典委托
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="key"></param>
/// <param name="value"></param>
/// <param name="reader"></param>
/// <param name="dictionary"></param>
public delegate bool JsonReadItemAction<TKey, TValue>(IJsonParser<TKey> key, IJsonParser<TValue> value, ref Utf8JsonReader reader, IDictionary<TKey, TValue> dictionary)
    where TKey : notnull;