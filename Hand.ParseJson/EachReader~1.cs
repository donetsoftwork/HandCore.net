using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// 多节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="item"></param>
public class EachReader<TResult>(IJsonParser<TResult> item)
     : WrapParser<TResult>(item), IJsonParser<List<TResult>>
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
    public virtual bool TryParse(ref Utf8JsonReader reader, out List<TResult> list)
    {
        var state = false;
        list = [];
        var currentDepth = reader.CurrentDepth;
        do
        {
            if (ReadItem(ref reader, list))
                state = true;
        } while (reader.Read() && reader.CurrentDepth >= currentDepth);
        return state;
    }
    /// <summary>
    /// 读取子项
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    protected virtual bool ReadItem(ref Utf8JsonReader reader, List<TResult> list)
    {
        if (_item.TryParse(ref reader, out var result))
        {
            list.Add(result);
            return true;
        }
        return false;
    }
}
