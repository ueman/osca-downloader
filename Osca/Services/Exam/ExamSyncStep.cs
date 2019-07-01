using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.Exam
{
    public class ExamSyncStep : ISyncStep
    {
        private readonly OscaAppService oscaAppService;
        private readonly DatabaseService databaseService;

        public List<object> NewEntities { get; } = new List<object>();
        public List<Exception> Exceptions { get; } = new List<Exception>();

        public string SyncStepName => "Noten";

        public ExamSyncStep(OscaAppService oscaAppService, DatabaseService databaseService)
        {
            this.oscaAppService = oscaAppService;
            this.databaseService = databaseService;
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            try
            {
                var downloadedExams = await oscaAppService.GetExams(cancellationToken);

                var examsFromDb = await databaseService.GetConnection()
                                                       .Table<StudentExam>()
                                                       .ToListAsync();

                var diff = downloadedExams.Except(examsFromDb).ToList();
                // in "diff" sind die Noten die sich geändert haben

                await databaseService.DropTableAndInsertAll(downloadedExams);
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
            }
        }
    }
}
