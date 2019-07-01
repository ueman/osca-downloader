using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Osca.Models.Canteens;
namespace Osca.Services.Food
{
	public class CanteenService
	{
		private HttpClient _client;

		public CanteenService()
		{
			_client = new HttpClient();
		}

		public async Task<List<Canteen>> LoadCanteens()
		{
			var canteenContainer = await GetParsed<Canteens>(CanteenUrls.Canteens);
			if (canteenContainer == null)
			{
				return new List<Canteen>();
			}
			var canteens = canteenContainer.Canteen;
			foreach (var canteen in canteens)
			{
				canteen.BusinessHours.CanteenId = canteen.Id;
			}
			return canteens;
		}

		public async Task<List<Dish>> LoadDishes(Canteen canteen)
		{
			var dishes = await GetParsed<Dishes>(CanteenUrls.FoodForCantine(canteen.Id));
			return dishes.Dish;
		}

		private async Task<T> GetParsed<T>(string url) where T : class
		{
			try
			{
				var content = await _client.GetAsync(url);
				var result = await Parse<T>(content.Content);
				return result;
			}
			catch (Exception e)
			{
				// todo hier was sinnvolles tun
				return null;
			}
		}

		private static async Task<T> Parse<T>(HttpContent httpContent)
		{
			using (var content = await httpContent.ReadAsStreamAsync())
			{
				var deserializer = new XmlSerializer(typeof(T));
				var result = (T)deserializer.Deserialize(content);
				return result;
			}
		}
	}
}
