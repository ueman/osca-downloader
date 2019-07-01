using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Course;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.FileArea
{
    /// <summary>
    /// Sync die Dateibereiche aller Kurse des aktuellsten Semesters.
    /// Setzt dabei vorraus, dass vorher auch Kurse gesynct worden sind.
    /// Ohne Kurse keine Dateibereiche.
    /// </summary>
    public class FileDownloadSyncStep : ISyncStep
    {
        private readonly FileAreaService fileAreaService;
        private readonly FileService _fileService;
        private readonly DatabaseService databaseService;
        private readonly CourseService courseService;
        private readonly OscaWebService oscaWebService;

        public string SyncStepName => "Datei download";

        public List<Exception> Exceptions { get; } = new List<Exception>();
        public List<object> NewEntities { get; } = new List<object>();

        public FileDownloadSyncStep(
            OscaWebService oscaWebService,
            DatabaseService databaseService,
            CourseService courseService,
            FileAreaService fileAreaService,
            FileService fileService)
        {
            this.fileAreaService = fileAreaService;
            this._fileService = fileService;
            this.oscaWebService = oscaWebService;
            this.databaseService = databaseService;
            this.courseService = courseService;
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            var courses = await courseService.GetCoursesForCurrentSemester();
            foreach (var course in courses)
            {
                try
                {
                    Console.WriteLine($"Lade Dateien für Kurs: {course.CourseName}");
                    var filesToDownload = await fileAreaService.GetFilesForCourse(course);
                    var downloadFileTasks = filesToDownload
                        .Select(it => _fileService.DownloadFile(it, course))
                        .ToList();
                    await Task.WhenAll(downloadFileTasks);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Beim laden der Dateien für {course.CourseName} ist ein Fehler aufgetreten");
                    Exceptions.Add(e);
                }
            }
        }
    }
}