using System;
using SQLite;
namespace Osca.Models.Osca
{
	public class CourseFile
	{
		public string CourseId { get; set; }

		public string Name { get; set; }

		[PrimaryKey]
		public string ServerRelativeUrl { get; set; }

		public string DownloadUrl => $"https://osca.hs-osnabrueck.de{ServerRelativeUrl}";

		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}
