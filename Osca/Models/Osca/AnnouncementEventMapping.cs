using SQLite;
namespace Osca.Models.Osca
{
	public class AnnouncementEventMapping
	{
		/// <summary>
		/// Zeigt auf die <see cref="StudentEvent.CourseID"/>
		/// </summary>
		/// <value>The event identifier.</value>
		[PrimaryKey]
		public string EventId { get; set; }

		public bool ShowInAnnouncements { get; set; }
	}
}
