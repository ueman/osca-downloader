using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;

namespace Osca.Services.Appointments
{
	public class AppointmentSyncStep : ISyncStep
	{
		private readonly OscaAppService oscaAppService;
		private readonly DatabaseService databaseService;

		public string SyncStepName => "Stundenplan";

		public List<Exception> Exceptions { get; } = new List<Exception>();
		public List<object> NewEntities { get; } = new List<object>();

		public AppointmentSyncStep(OscaAppService oscaAppService, DatabaseService databaseService)
		{
			this.oscaAppService = oscaAppService;
			this.databaseService = databaseService;
		}

		public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var appointments = await oscaAppService.GetAppointments(cancellationToken);
				await databaseService.DropTableAndInsertAll(appointments);
			}
			catch (Exception e)
			{
				Exceptions.Add(e);
			}
		}
	}
}
