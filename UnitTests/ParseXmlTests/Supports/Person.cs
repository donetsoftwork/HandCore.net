using System.Xml.Serialization;

namespace ParseXmlTests.Supports;

//[Serializable]
[XmlRoot("Root")]
public class Person
{
    [XmlAttribute("PersonId")]
    public int Id { get; set; }
    [XmlElement("PersonName")]
    public string Name { get; set; }
}
