using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Osca.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Startet einen Task ohne das er awaited werden muss. Falls er Exceptions wirft werden diese 
        /// gefangen und "behandelt".
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="caller">Caller.</param>
        /// <param name="lineNumber">Line number.</param>
        /// <param name="path">Path.</param>
        public static void FireAndForget(
            this Task task,
            [CallerMemberName] string caller = "Unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string path = "???"
        )
        {
            // Dieser Aufruf soll asynchron im Hintergrund laufen und nicht awaited werden
            task.ContinueWith(t => HandleTaskFailure(t, caller, lineNumber, path), TaskContinuationOptions.OnlyOnFaulted);
        }

        private static void HandleTaskFailure(Task task, string caller, int lineNumber, string path)
        {
            var source = $"{Path.GetFileName(path)}#{caller}@{lineNumber}";
            task.Exception.Handle(e =>
            {
                Debug.WriteLine($"Task called at {source} failed: {e}");
                return true;
            });
            Debugger.Break();
        }

        public static void FireAndForgetOnMainThread(
            this Task task,
            [CallerMemberName] string caller = "Unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string path = "???"
        )
        {
            FireAndForget(task, caller, lineNumber, path);
        }

        public static ConfiguredTaskAwaitable AnyContext(this Task t)
            => t.ConfigureAwait(false);

        public static ConfiguredTaskAwaitable<T> AnyContext<T>(this Task<T> t)
            => t.ConfigureAwait(false);
    }
}
