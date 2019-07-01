using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.Course
{
	public class CourseSyncStep : ISyncStep
	{
		private readonly OscaAppService oscaAppService;
		private readonly DatabaseService databaseService;

		public string SyncStepName => "Kurse";

		public List<Exception> Exceptions { get; } = new List<Exception>();
		public List<object> NewEntities { get; } = new List<object>();

		public CourseSyncStep(OscaAppService oscaAppService, DatabaseService databaseService)
		{
			this.oscaAppService = oscaAppService;
			this.databaseService = databaseService;
		}

		public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var courses = await oscaAppService.GetEvents(cancellationToken);
				await databaseService.DropTableAndInsertAll(courses);
			}
			catch (Exception e)
			{
				Exceptions.Add(e);
			}
		}
	}
}
