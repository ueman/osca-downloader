using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Course;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.Announcements
{
	/// <summary>
	/// Synct alle Ankündigungen des aktuellsten Semesters.
	/// Setzt dabei vorraus, das vorher auch Kurse gesynct worden sind.
	/// Ohne Kurse keine Ankündigungen.
	/// </summary>
	public class AnnouncementSyncStep : ISyncStep
	{
		private readonly OscaWebService oscaWebService;
		private readonly AnnouncementService announcementService;
		private readonly DatabaseService databaseService;
		private readonly CourseService courseService;

		public List<Exception> Exceptions { get; } = new List<Exception>();
		public List<object> NewEntities { get; } = new List<object>();

		public string SyncStepName => "Ankündigungen";

		public AnnouncementSyncStep(OscaWebService oscaWebService,
									AnnouncementService announcementService,
									DatabaseService databaseService,
									CourseService courseService)
		{
			this.oscaWebService = oscaWebService;
			this.announcementService = announcementService;
			this.databaseService = databaseService;
			this.courseService = courseService;
		}

		public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var courses = await courseService.GetCoursesForCurrentSemester();
				var announcements = await oscaWebService.GetAnouncementsForCourses(courses);
				await databaseService.DropTableAndInsertAll(announcements);
			}
			catch (Exception e)
			{
				Exceptions.Add(e);
			}
		}
	}
}
