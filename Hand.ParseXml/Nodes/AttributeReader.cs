using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 文本读取器
/// </summary>
public class AttributeReader(string name, string defaultValue = "")
    : ValueReader<string>(defaultValue)
{
    #region 配置
    private readonly string _name = name;

    /// <summary>
    /// 属性名
    /// </summary>
    public string Name
        => _name;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out string result)
    {
        var value = reader.GetAttribute(_name);
        if(value is null)
        {
            result = _defaultValue;
            return false;
        }
        result = value;
        return true;
    }
}
