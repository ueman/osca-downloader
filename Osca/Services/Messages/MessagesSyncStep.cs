using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Database;
using Osca.Services.Osca;
using Osca.Services.Sync;
namespace Osca.Services.Messages
{
	public class MessagesSyncStep : ISyncStep
	{
		private readonly OscaAppService oscaAppService;
		private readonly DatabaseService databaseService;

		public MessagesSyncStep(OscaAppService oscaAppService, DatabaseService databaseService)
		{
			this.oscaAppService = oscaAppService;
			this.databaseService = databaseService;
		}

		public string SyncStepName => "Nachrichten";

		public List<Exception> Exceptions { get; } = new List<Exception>();
		public List<object> NewEntities { get; } = new List<object>();

		public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var messages = await oscaAppService.GetMessages();
				await databaseService.DropTableAndInsertAll(messages);
			}
			catch (Exception e)
			{
				Exceptions.Add(e);
			}
		}
	}
}