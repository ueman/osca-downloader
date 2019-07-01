using System;
using Newtonsoft.Json;
using Osca.JsonConverter;
using SQLite;
namespace Osca.Models.Osca
{
	public class Announcement
	{
		/// <summary>
		/// Unique ID, nur für die Datenbank
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public Guid Id { get; set; }

		public string Title { get; set; }

		/// <summary>
		/// Url zum eigentlichen Announcement aus dem dieser POCO herausgeparst wurde
		/// </summary>
		/// <value>The URL.</value>
		[SQLite.Ignore]
		public string Url => $"https://osca.hs-osnabrueck.de/lms/{CourseId}/Lists/ank/DispForm.aspx?ID={AnnouncementId}";

		/// <summary>
		/// Inhalt der Ankündigung
		/// </summary>
		/// <value>The body.</value>
		public string Body { get; set; }

		/// <summary>
		/// <see cref="StudentEvent.CourseID"/>
		/// </summary>
		/// <value>The course identifier.</value>
		public string CourseId { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		/// <value>The created date.</value>
		[JsonConverter(typeof(DateTimeJsonConverter))]
		public DateTime Created { get; set; }

		/// <summary>
		/// Datum an dem die Ankündigung geändert wurde
		/// </summary>
		/// <value>The modified date.</value>
		[JsonConverter(typeof(DateTimeJsonConverter))]
		public DateTime Modified { get; set; }

		/// <summary>
		/// Gibt an, die wievielte Ankündigung dies ist.
		/// </summary>
		/// <value>The announcement identifier.</value>
		public int AnnouncementId { get; set; }
	}

	public class DisplayAnnouncement : Announcement
	{
		public static string CreateView => @"
CREATE VIEW IF NOT EXISTS DisplayAnnouncement AS
SELECT Announcement.Id, Announcement.Title, Announcement.Body, Announcement.CourseId, Announcement.Created, Announcement.Modified, Announcement.AnnouncementId, StudentEvent.CourseName
FROM Announcement
LEFT JOIN StudentEvent 
ON StudentEvent.CourseID = Announcement.CourseId
";

		public string CourseName { get; set; }
	}
}
