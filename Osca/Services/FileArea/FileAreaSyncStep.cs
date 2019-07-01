using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Course;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.FileArea
{
    /// <summary>
    /// Synct die Dateibereiche aller Kurse des aktuellsten Semesters.
    /// Setzt dabei vorraus, dass vorher auch Kurse gesynct worden sind.
    /// Ohne Kurse keine Dateibereiche.
    /// </summary>
    public class FileAreaSyncStep : ISyncStep
    {
        private readonly DatabaseService databaseService;
        private readonly CourseService courseService;
        private readonly OscaWebService oscaWebService;

        public string SyncStepName => "Dateibereich";

        public List<Exception> Exceptions { get; } = new List<Exception>();
        public List<object> NewEntities { get; } = new List<object>();

        public FileAreaSyncStep(OscaWebService oscaWebService,
                                DatabaseService databaseService,
                                CourseService courseService)
        {
            this.oscaWebService = oscaWebService;
            this.databaseService = databaseService;
            this.courseService = courseService;
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            try
            {
                var courses = await courseService.GetCoursesForCurrentSemester();
                var files = await oscaWebService.GetFilesForCourses(courses, cancellationToken);
                await databaseService.DropTableAndInsertAll(files);
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
            }
        }
    }
}
