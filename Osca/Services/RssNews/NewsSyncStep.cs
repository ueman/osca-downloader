using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Osca.Services.Database;
using Osca.Services.Sync;
namespace Osca.Services.RssNews
{
	public class NewsSyncStep : ISyncStep
	{
		private readonly NewsService newsService;
		private readonly DatabaseService databaseService;

		public NewsSyncStep(NewsService newsService, DatabaseService databaseService)
		{
			this.newsService = newsService;
			this.databaseService = databaseService;
		}

		public string SyncStepName => "News";

		public List<Exception> Exceptions { get; } = new List<Exception>();
		public List<object> NewEntities { get; } = new List<object>();

		public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var items = await newsService.LoadNews();
				await databaseService.DropTableAndInsertAll(items);
				await newsService.DownloadAndSaveImages(items);
			}
			catch (Exception e)
			{
				Exceptions.Add(e);
			}
		}
	}
}
