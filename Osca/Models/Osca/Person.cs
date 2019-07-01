using System.Xml.Serialization;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "person", Namespace = "http://datenlotsen.de")]
	public class Person
	{
		[XmlElement(ElementName = "actortype", Namespace = "http://datenlotsen.de")]
		public string Actortype { get; set; }
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class PersonMessage
	{
		[XmlElement(ElementName = "person", Namespace = "http://datenlotsen.de")]
		public Person Person { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
	}
}
