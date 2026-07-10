using Hand.Convert;
using System.Xml;

namespace Hand.ParseXml.Contracts;

/// <summary>
/// 实体解析器
/// </summary>
public interface IEntityParser<TEntity>
    : IEntityParser, IParser<XmlReader, TEntity>
{
}
