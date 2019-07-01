using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Canteens
{
	[XmlRoot(ElementName = "canteen", Namespace = "http://www.datenlotsen.de")]
	public class Canteen
	{
		[XmlElement(ElementName = "name", Namespace = "http://www.datenlotsen.de")]
		public string Name { get; set; }

		[XmlElement(ElementName = "street", Namespace = "http://www.datenlotsen.de")]
		public string Street { get; set; }

		[XmlElement(ElementName = "zipcode", Namespace = "http://www.datenlotsen.de")]
		public string ZipCode { get; set; }

		[XmlElement(ElementName = "city", Namespace = "http://www.datenlotsen.de")]
		public string City { get; set; }

		[Ignore]
		[XmlElement(ElementName = "businessHours", Namespace = "http://www.datenlotsen.de")]
		public BusinessHours BusinessHours { get; set; }

		[XmlElement(ElementName = "latitude", Namespace = "http://www.datenlotsen.de")]
		public double Latitude { get; set; }

		[XmlElement(ElementName = "longitude", Namespace = "http://www.datenlotsen.de")]
		public double Longitude { get; set; }

		[XmlElement(ElementName = "photoURL", Namespace = "http://www.datenlotsen.de")]
		public string PhotoURL { get; set; }

		[XmlElement(ElementName = "website", Namespace = "http://www.datenlotsen.de")]
		public string Website { get; set; }

		[PrimaryKey]
		[XmlAttribute(AttributeName = "id")]
		public int Id { get; set; }
	}

	[XmlRoot(ElementName = "canteens", Namespace = "http://www.datenlotsen.de")]
	public class Canteens
	{
		[XmlElement(ElementName = "canteen", Namespace = "http://www.datenlotsen.de")]
		public List<Canteen> Canteen { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }

		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
	}

	[XmlRoot(ElementName = "businessHours", Namespace = "http://www.datenlotsen.de")]
	public class BusinessHours
	{
		[PrimaryKey]
		[XmlIgnore]
		public int CanteenId { get; set; }

		[XmlElement(ElementName = "monday", Namespace = "http://www.datenlotsen.de")]
		public string Monday { get; set; }

		[XmlElement(ElementName = "tuesday", Namespace = "http://www.datenlotsen.de")]
		public string Tuesday { get; set; }

		[XmlElement(ElementName = "wednesday", Namespace = "http://www.datenlotsen.de")]
		public string Wednesday { get; set; }

		[XmlElement(ElementName = "thursday", Namespace = "http://www.datenlotsen.de")]
		public string Thursday { get; set; }

		[XmlElement(ElementName = "friday", Namespace = "http://www.datenlotsen.de")]
		public string Friday { get; set; }

		[XmlElement(ElementName = "saturday", Namespace = "http://www.datenlotsen.de")]
		public string Saturday { get; set; }

		[XmlElement(ElementName = "sunday", Namespace = "http://www.datenlotsen.de")]
		public string Sunday { get; set; }
	}
}
