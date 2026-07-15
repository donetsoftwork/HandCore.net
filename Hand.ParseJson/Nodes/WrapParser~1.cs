using Hand.ParseJson.Contracts;
using Hand.Structural;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 包装解析器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="original"></param>
public abstract class WrapParser<TValue>(IJsonParser<TValue> original)
    : IWrapper<IJsonParser<TValue>>
{
    #region 配置
    /// <summary>
    /// 原始解析器
    /// </summary>
    protected readonly IJsonParser<TValue> _original = original;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IJsonParser<TValue> Original
        => _original;
    #endregion
}
