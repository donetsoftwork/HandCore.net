using System.Xml;

namespace Hand.ParseXml.Contracts;

/// <summary>
/// Xml解析接口
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IXmlParser<TResult>
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryParser(XmlReader reader, out TResult result);
}
