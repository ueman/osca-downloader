using System;
namespace Osca.Services.Osca.Magic
{
	public static class OscaUrlBuilder
	{
		public enum PersonType
		{
			/// <summary>
			/// Wert: STD
			/// </summary>
			Student,
			/// <summary>
			/// Wert: DOZ
			/// </summary>
			Dozent
		}

		private static string BaseUrl = "https://osca-bew.hs-osnabrueck.de/scripts/mgrqispi.dll?APPNAME=CampusNet&PRGNAME=ACTIONMOBILE&ARGUMENTS=";

		public static string PersonTypeUrl(string sessionId)
		{
			string url = "GETPERSONTYPE," + sessionId + ",000000,1";
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string AppointmentsUrl(string sessionId)
		{
			string url = "GETAPPOINTMENTS" + "," + sessionId + ",000000,";
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string MessagesUrl(string sessionId)
		{
			string url = "GETMESSAGES" + "," + sessionId + ",000000,";
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string EventInfoUrl(string sessionId, string eventId)
		{

			string url = "GETEVENTINFO" + "," + sessionId + ",000000," + eventId;
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string EventsUrl(string sessionId, PersonType personType)
		{
			string url = "GETEVENTS" + "," + sessionId + ",000000," + PersontTypeToString(personType);
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string ExamsUrl(string sessionId, PersonType personType)
		{
			string url = "GETEXAMS" + "," + sessionId + ",000000," + PersontTypeToString(personType);
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string GetMaterialsUrl(string sessionId, string objectId /* StudentEvent.id oder Appointment.id */, string objectType /*EVENT oder TIMETABLE */)
		{
			string url = "GETMATERIAL" + "," + sessionId + ",000000," + objectId + "," + objectType;
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		public static string GetEventDownloadUrl(string sessionId, string irgendwasVomMaterial)
		{
			string url = "GETEVENTDOWNLOAD" + "," + sessionId + ",000000," + irgendwasVomMaterial;
			string urlWithMd5 = StringUtil.StringToMd5(url) + "," + url;
			return BaseUrl + "-A" + Encoder.DoMagic(urlWithMd5);
		}

		private static string PersontTypeToString(PersonType type)
		{
			switch (type)
			{
				case PersonType.Dozent: return "DOZ";
				case PersonType.Student: return "STD";
			}
			throw new InvalidOperationException("PersonType kann nur Dozent oder Student sein");
		}
	}
}