using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Announcements;
using Osca.Services.Appointments;
using Osca.Services.Course;
using Osca.Services.Database;
using Osca.Services.Exam;
using Osca.Services.FileArea;
using Osca.Services.Food;
using Osca.Services.Messages;
using Osca.Services.Osca;
using Osca.Services.RssNews;

namespace Osca.Services.Sync
{
    public class SyncService
    {
        private readonly OscaAppService _oscaAppService;
        private readonly DatabaseService _databaseService;
        private readonly CanteenService _canteenService;
        private readonly AnnouncementService _announcementService;
        private readonly OscaWebService _oscaWebService;
        private readonly CourseService _courseService;
        private readonly NewsService _newsService;
        private readonly FileAreaService _fileAreaService;
        private readonly FileService _fileService;

        public SyncService(
            DatabaseService databaseService,
            OscaAppService oscaAppService,
            OscaWebService oscaWebService,
            AnnouncementService announcementService,
            CourseService courseService,
            CanteenService canteenService,
            NewsService newsService,
            FileAreaService fileAreaService,
            FileService fileService
            )
        {
            _databaseService = databaseService;
            _oscaAppService = oscaAppService;
            _canteenService = canteenService;
            _announcementService = announcementService;
            _oscaWebService = oscaWebService;
            _courseService = courseService;
            _newsService = newsService;
            _fileAreaService = fileAreaService;
            _fileService = fileService;
        }

        public async Task SyncAll(
            Action<string, int, int> progressCallback,
            Action<List<Exception>> finishedCallback,
            Action<List<object>> newEntitiesSynced,
            CancellationToken cancellationToken = default)
        {
            var syncSteps = BuildSyncSteps();
            var stepCount = syncSteps.Count;
            var count = 0;
            var exceptions = new List<Exception>();

            foreach (var step in syncSteps)
            {
                count = count + 1;
                progressCallback(step.SyncStepName, count, stepCount);
                // Alle Schritte werden nacheinander ausgeführt,
                // da auch in die DB geschrieben wird.
                // Dies geht mit SQLite nicht parallel
                await step.Run(cancellationToken);
                exceptions.AddRange(step.Exceptions);
                newEntitiesSynced(step.NewEntities);
            }
            finishedCallback(exceptions);
        }

        public List<ISyncStep> BuildSyncSteps()
        {
            return new List<ISyncStep>
            {
                new NewsSyncStep(_newsService, _databaseService),
                new AppointmentSyncStep(_oscaAppService, _databaseService),
                new CourseSyncStep(_oscaAppService, _databaseService),
                new ExamSyncStep(_oscaAppService, _databaseService),
                new MessagesSyncStep(_oscaAppService, _databaseService),
                new GenericSyncStep(_oscaAppService, _databaseService, _canteenService),
				// Die folgenden SyncSteps erst nach den oberen ausführen, 
				// da sie darauf aufbauen
                new AnnouncementSyncStep(_oscaWebService, _announcementService, _databaseService, _courseService),
				new FileAreaSyncStep(_oscaWebService, _databaseService, _courseService),
                new FileDownloadSyncStep(_oscaWebService, _databaseService, _courseService, _fileAreaService, _fileService),
            };
        }
    }

    public class GenericSyncStep : ISyncStep
    {
        private readonly OscaAppService oscaAppService;
        private readonly DatabaseService databaseService;
        private readonly CanteenService canteenService;

        public string SyncStepName => "Mensa";

        public List<Exception> Exceptions { get; } = new List<Exception>();
        public List<object> NewEntities { get; } = new List<object>();

        public GenericSyncStep(OscaAppService oscaAppService,
                               DatabaseService databaseService,
                               CanteenService canteenService)
        {
            this.oscaAppService = oscaAppService;
            this.databaseService = databaseService;
            this.canteenService = canteenService;
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            try
            {
                var canteens = await canteenService.LoadCanteens();
                /*
				var dishes = new List<Dish>();
				foreach (var canteen in canteens)
				{
					dishes.AddRange(await _canteenService.LoadDishes(canteen));
				}
				*/
                await databaseService.DropTableAndInsertAll(canteens);
                //await _databaseService.DropTableAndInsertAll(dishes);
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
            }
        }
    }
}
