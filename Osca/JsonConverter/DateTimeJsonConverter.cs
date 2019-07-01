using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Osca.JsonConverter
{
	public class DateTimeJsonConverter : JsonConverter<DateTime>
	{
		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			string dateString = (string)reader.Value;
			var date = DateTime.ParseExact(dateString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
			return date;
		}

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
