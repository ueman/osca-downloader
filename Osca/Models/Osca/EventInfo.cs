using System.Collections.Generic;
using System.Xml.Serialization;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "eventInfo", Namespace = "http://datenlotsen.de")]
	public class EventInfo
	{
		[XmlElement(ElementName = "subject", Namespace = "http://datenlotsen.de")]
		public string Subject { get; set; }
		[XmlElement(ElementName = "text", Namespace = "http://datenlotsen.de")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class EventInfoMessage
	{
		[XmlElement(ElementName = "eventInfo", Namespace = "http://datenlotsen.de")]
		public List<EventInfo> EventInfo { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
	}
}
