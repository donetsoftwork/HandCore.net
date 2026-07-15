using Hand.Creational;
using Hand.ParseXml;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace ParseXmlTests.Supports;

/// <summary>
/// 注释解析器
/// </summary>
/// <param name="xml"></param>
public class CommentParser(HandXml xml)
    : EntityParser<Comment>(xml, CommentBuilder.Creater, null, true)
{
    #region 配置
    private readonly ContentReader _content = xml.Content();
    private readonly AttributeReader _name = xml.Attribute("name");
    #endregion
    /// <inheritdoc />
    public override void ReadAttributes(IMemberStore entity, XmlReader reader) { }
    /// <inheritdoc />
    public override void ReadItem(IMemberBuilder<Comment> entity, XmlReader reader)
    {
        if (entity is CommentBuilder builder)
            ReadItem(builder.Original, reader);
        else
            base.ReadItem(entity, reader);
    }
    /// <summary>
    /// 使用自定义构造器
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    public void ReadItem(Comment entity, XmlReader reader)
    {
        string name = reader.LocalName;
        switch (name)
        {
            case "summary":
                if (_content.TryParse(reader, out var summaryResult))
                    entity.Summary = summaryResult;
                break;
            case "typeparam":
                ReadDictionary(reader, entity.TypeParams);
                break;
            case "param":
                ReadDictionary(reader, entity.Params);
                break;
            case "returns":
                if (_content.TryParse(reader, out var returnsResult))
                    entity.Returns = returnsResult;
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="dic"></param>
    public void ReadDictionary(XmlReader reader, Dictionary<string, string> dic)
    {
        if(_name.TryParse(reader, out var name))
        {
            var value = _content.Parse(reader);
            dic[name] = value;
        }
    }
}
