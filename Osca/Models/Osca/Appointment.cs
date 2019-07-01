using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "appointment", Namespace = "http://datenlotsen.de")]
	public class Appointment
	{
		[PrimaryKey]
		[XmlElement(ElementName = "timetableID", Namespace = "http://datenlotsen.de")]
		public string TimetableID { get; set; }

		[XmlElement(ElementName = "timetableDate", Namespace = "http://datenlotsen.de")]
		public string TimetableDate { get; set; }

		[XmlElement(ElementName = "timeFromFull", Namespace = "http://datenlotsen.de")]
		public string TimeFromFull { get; set; }

		[XmlElement(ElementName = "timeToFull", Namespace = "http://datenlotsen.de")]
		public string TimeToFull { get; set; }

		[XmlElement(ElementName = "roomString", Namespace = "http://datenlotsen.de")]
		public string RoomString { get; set; }

		[XmlElement(ElementName = "timeFrom", Namespace = "http://datenlotsen.de")]
		public string TimeFrom { get; set; }

		[XmlElement(ElementName = "timeTo", Namespace = "http://datenlotsen.de")]
		public string TimeTo { get; set; }

		[XmlElement(ElementName = "appointmentName", Namespace = "http://datenlotsen.de")]
		public string AppointmentName { get; set; }

		[XmlElement(ElementName = "appointmentType", Namespace = "http://datenlotsen.de")]
		public string AppointmentType { get; set; }

		[XmlElement(ElementName = "instructorString", Namespace = "http://datenlotsen.de")]
		public string InstructorString { get; set; }

		[XmlElement(ElementName = "materialPresent", Namespace = "http://datenlotsen.de")]
		public string MaterialPresent { get; set; }

		[XmlElement(ElementName = "appointmentNameShort", Namespace = "http://datenlotsen.de")]
		public string AppointmentNameShort { get; set; }

		public string FormattedDay => StartDate?.ToString("ddd, dd MMM yyyy");

		[XmlIgnore]
		public DateTime? StartDate
		{
			get
			{
				try
				{
					var completeStartTime = $"{TimetableDate} {TimeFromFull}";
					if (string.IsNullOrWhiteSpace(completeStartTime))
					{
						return null;
					}
					var date = DateTime.ParseExact(completeStartTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
					return date;
				}
				catch
				{
					return null;
				}
			}
			// Damit das Datum in die DB geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}

		[XmlIgnore]
		public DateTime? EndDate
		{
			get
			{
				try
				{
					var completeEndTime = $"{TimetableDate} {TimeToFull}";
					if (string.IsNullOrWhiteSpace(completeEndTime))
					{
						return null;
					}
					var date = DateTime.ParseExact(completeEndTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
					return date;
				}
				catch
				{
					return null;
				}
			}
			// Damit das Datum in die DB geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class AppointmentMessage
	{
		[XmlElement(ElementName = "appointment", Namespace = "http://datenlotsen.de")]
		public List<Appointment> Appointment { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
	}
}
