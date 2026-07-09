using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 多节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="json"></param>
/// <param name="item"></param>
public class RepeatReader<TResult>(HandJson json, IJsonParser<TResult> item)
     : WrapParser<TResult>(json, item), IJsonParser<List<TResult>>
{
    #region 配置
    private readonly IJsonParser<TResult> _item = item;
    /// <summary>
    /// 子元素解析器
    /// </summary>
    public IJsonParser<TResult> Item
        => _item;
    #endregion

    /// <inheritdoc />
    public bool TryParser(ref Utf8JsonReader reader, out List<TResult> list)
    {        
        var currentDepth = reader.CurrentDepth;
        list = [];
        do
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    if (ReadList(ref reader, list))
                        return true;
                    break;
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    // 检查是否到达结束节点
                    if (reader.CurrentDepth <= currentDepth)
                        return false;
                    break;
                default:
                    break;
            }
        }
        while (reader.Read());
        return false;
    }
    /// <summary>
    /// 读取列表
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="list"></param>
    public bool ReadList(ref Utf8JsonReader reader, List<TResult> list)
    {
        var depth = reader.CurrentDepth;
        var state = false;
        while (reader.Read())
        {
            if (reader.CurrentDepth <= depth)
                break;
            if (!_item.TryParser(ref reader, out var result))
                break;
            list.Add(result);
            state = true; 
        }
        return state;
    }
}
