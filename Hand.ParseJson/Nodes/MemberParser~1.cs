using Hand.Creational;
using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 成员处理器
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="name"></param>
/// <param name="original"></param>
public class MemberParser<TMember>(string name, IJsonParser<TMember> original)
    : IMemberParser
{
    #region 配置
    private readonly string _name = name;
    private readonly IJsonParser<TMember> _original = original;

    /// <summary>
    /// 成员名
    /// </summary>
    public string Name
        => _name;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IJsonParser<TMember> Original 
        => _original;
    #endregion

    /// <inheritdoc />
    public void Save(IMemberStore entity, ref Utf8JsonReader reader)
    {
        if (_original.TryParser(ref reader, out var value))
            entity.Save(_name, value);
    }
}
