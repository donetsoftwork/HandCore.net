using Hand.ParseJson.Contracts;
using Hand.Structural;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 包装解析器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="json"></param>
/// <param name="original"></param>
public abstract class WrapParser<TValue>(HandJson json, IJsonParser<TValue> original)
    : IWrapper<IJsonParser<TValue>>
{
    #region 配置
    /// <summary>
    /// json解析器
    /// </summary>
    protected readonly HandJson _json = json;
    /// <summary>
    /// 原始解析器
    /// </summary>
    protected readonly IJsonParser<TValue> _original = original;
    /// <inheritdoc />
    public HandJson Json
        => _json;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IJsonParser<TValue> Original
        => _original;
    #endregion
}
