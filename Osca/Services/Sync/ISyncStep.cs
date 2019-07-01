using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Osca.Services.Sync
{
	public interface ISyncStep
	{
		string SyncStepName { get; }

		List<Exception> Exceptions { get; }

		List<object> NewEntities { get; }

		Task Run(CancellationToken cancellationToken = default(CancellationToken));
	}
}
