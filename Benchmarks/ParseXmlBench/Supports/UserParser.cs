using Hand.Creational;
using Hand.ParseXml;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace ParseXmlBench.Supports;

public class UserParser(HandXml xml)
    : EntityParser<User>(xml, UserBuilder2.Creater, null, true)
{
    #region 配置
    private readonly IXmlParser<int> _id = xml.Content<int>();
    private readonly ContentReader _name = xml.Content();
    private readonly IXmlParser<int> _age = xml.Content<int>();
    #endregion
    /// <inheritdoc />
    public override void ReadAttributes(IMemberStore entity, XmlReader reader) { }
    /// <inheritdoc />
    public override void ReadItem(IMemberBuilder<User> entity, XmlReader reader, string name)
    {
        if (entity is UserBuilder2 builder)
            ReadItem(builder.Original, reader, name);
        else
            base.ReadItem(entity, reader, name);
    }
    /// <summary>
    /// 使用自定义构造器
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    public void ReadItem(User entity, XmlReader reader, string name)
    {        
        switch (name)
        {
            case nameof(User.Id):
                if (_id.TryParser(reader, out var idResult))
                    entity.Id = idResult;
                break;
            case nameof(User.Name):
                if (_name.TryParser(reader, out var nameResult))
                    entity.Name = nameResult;
                break;
            case nameof(User.Age):
                if (_age.TryParser(reader, out var ageResult))
                    entity.Age = ageResult;
                break;
        }
    }
}
