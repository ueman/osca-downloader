using System.Collections.Generic;
using System.Xml.Serialization;

namespace Osca.Models.Canteens
{
	[XmlRoot(ElementName = "additive", Namespace = "http://www.datenlotsen.de")]
	public class Additive
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlAttribute(AttributeName = "icon")]
		public string Icon { get; set; }
	}

	[XmlRoot(ElementName = "additivesGroup", Namespace = "http://www.datenlotsen.de")]
	public class AdditivesGroup
	{
		[XmlElement(ElementName = "additive", Namespace = "http://www.datenlotsen.de")]
		public List<Additive> Additive { get; set; }

		[XmlAttribute(AttributeName = "group")]
		public string Group { get; set; }
	}

	[XmlRoot(ElementName = "additives", Namespace = "http://www.datenlotsen.de")]
	public class Additives
	{
		[XmlElement(ElementName = "additivesGroup", Namespace = "http://www.datenlotsen.de")]
		public AdditivesGroup AdditivesGroup { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }

		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
	}

}
