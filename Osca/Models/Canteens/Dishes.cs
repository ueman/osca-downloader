using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Canteens
{
	[XmlRoot(ElementName = "additives", Namespace = "http://www.datenlotsen.de")]
	public class DishAdditives
	{
		[XmlElement(ElementName = "additive", Namespace = "http://www.datenlotsen.de")]
		public List<string> Additive { get; set; }
	}

	[XmlRoot(ElementName = "dish", Namespace = "http://www.datenlotsen.de")]
	public class Dish
	{
		[XmlElement(ElementName = "name", Namespace = "http://www.datenlotsen.de")]
		public string Name { get; set; }

		[XmlElement(ElementName = "category", Namespace = "http://www.datenlotsen.de")]
		public string Category { get; set; }

		[XmlElement(ElementName = "day", Namespace = "http://www.datenlotsen.de")]
		public string Day { get; set; }

		[XmlElement(ElementName = "priceGuest", Namespace = "http://www.datenlotsen.de")]
		public string PriceGuest { get; set; }

		[XmlElement(ElementName = "priceOfficial", Namespace = "http://www.datenlotsen.de")]
		public string PriceOfficial { get; set; }

		[XmlElement(ElementName = "priceStudent", Namespace = "http://www.datenlotsen.de")]
		public string PriceStudent { get; set; }

		[Ignore]
		[XmlElement(ElementName = "additives", Namespace = "http://www.datenlotsen.de")]
		public DishAdditives Additives { get; set; }

		[PrimaryKey]
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "dishes", Namespace = "http://www.datenlotsen.de")]
	public class Dishes
	{
		[XmlElement(ElementName = "dish", Namespace = "http://www.datenlotsen.de")]
		public List<Dish> Dish { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }

		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }

		[XmlAttribute(AttributeName = "canteenid")]
		public string Canteenid { get; set; }

		[XmlAttribute(AttributeName = "additivesGroup")]
		public string AdditivesGroup { get; set; }
	}

}
