using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "message", Namespace = "http://datenlotsen.de")]
	public class Message
	{
		[XmlElement(ElementName = "subject", Namespace = "http://datenlotsen.de")]
		public string Subject { get; set; }

		[XmlElement(ElementName = "body", Namespace = "http://datenlotsen.de")]
		public string Body { get; set; }

		[XmlElement(ElementName = "mailDate", Namespace = "http://datenlotsen.de")]
		public string MailDate { get; set; }

		[XmlElement(ElementName = "mailTime", Namespace = "http://datenlotsen.de")]
		public string MailTime { get; set; }

		[XmlElement(ElementName = "mailFolder", Namespace = "http://datenlotsen.de")]
		public string MailFolder { get; set; }

		[XmlElement(ElementName = "mailNew", Namespace = "http://datenlotsen.de")]
		public string MailNew { get; set; }

		[PrimaryKey]
		[XmlElement(ElementName = "messageID", Namespace = "http://datenlotsen.de")]
		public string MessageID { get; set; }

		[XmlElement(ElementName = "personFrom", Namespace = "http://datenlotsen.de")]
		public string PersonFrom { get; set; }

		[XmlElement(ElementName = "personTo", Namespace = "http://datenlotsen.de")]
		public string PersonTo { get; set; }

		[XmlIgnore]
		public DateTime Date
		{
			get
			{
				var completeTime = $"{MailDate} {MailTime}";
				var date = DateTime.ParseExact(completeTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
				return date;
			}
			// muss da sein, damit es in die Datenbank geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class MessageMessage
	{
		[XmlElement(ElementName = "message", Namespace = "http://datenlotsen.de")]
		public List<Message> Message { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
	}
}
