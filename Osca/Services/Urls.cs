namespace Osca.Services
{
	public static class CanteenUrls
	{
		public static string Canteens = "http://appdata.osca.hs-osnabrueck.de/CampusnetApp/dishes/canteens.xml";

		public static string Additives = "http://appdata.osca.hs-osnabrueck.de/CampusnetApp/dishes/additives.xml";

		public static string FoodForCantine(int id)
		{
			return $"http://appdata.osca.hs-osnabrueck.de/CampusnetApp/dishes/dishes_{id}.xml";
		}
	}

	public static class NewsUrls
	{
		public static string HochschulNews = "https://www.hs-osnabrueck.de/de/wir/wir-stellen-uns-vor/nachrichten/?type=9818";
	}
}
