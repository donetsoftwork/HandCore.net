using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 文本读取器
/// </summary>
public class IndexAttributeReader(int index, string defaultValue = "")
    : ValueReader<string>(defaultValue)
{
    #region 配置
    private readonly int _index = index;

    /// <summary>
    /// 属性索引
    /// </summary>
    public int Index
        => _index;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out string result)
    {
        var value = reader.GetAttribute(_index);
        if (value is null)
        {
            result = _defaultValue;
            return false;
        }
        result = value;
        return true;
    }
}
