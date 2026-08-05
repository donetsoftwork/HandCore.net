using System.Xml.Serialization;

namespace ParseXmlTests.Supports;

//[Serializable]
[XmlRoot("Root")]
public class Person
{
    [XmlAttribute("PersonId")]
    public int Id { get; set; }
    //[XmlElement("PersonAge")]
    //public int Age { get; set; }
    [XmlText]
    public string Name { get; set; }
}
