using System;
using System.IO;
using SQLite;

namespace Osca.Models.News
{
	public class RssItem
	{
		public string Title { get; set; }

		public string Link { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		public string Comments { get; set; }

		public string ImageUrl { get; set; }

		public string FileName
		{
			get
			{
				var url = new Uri(ImageUrl);
				var fileName = Path.GetFileName(url.LocalPath);
				return fileName;
			}
		}

		public string PathToFile
		{
			get
			{
				var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images");
				path = Path.Combine(path, FileName);
				return path;
			}
		}


		[PrimaryKey]
		public string Guid { get; set; }

		public string PublishingDateString { get; set; }

		/// <summary>
		/// The "pubDate" field as DateTime. Null if parsing failed or pubDate is empty.
		/// </summary>
		public DateTime? PublishingDate { get; set; }

		public string Content { get; set; }
	}
}
