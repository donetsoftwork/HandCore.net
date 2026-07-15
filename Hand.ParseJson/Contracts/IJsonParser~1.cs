using System.Text.Json;

namespace Hand.ParseJson.Contracts;

/// <summary>
/// Json解析接口
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IJsonParser<TResult>
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryParse(ref Utf8JsonReader reader, out TResult result);
}
