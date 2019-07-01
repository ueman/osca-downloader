using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Osca.Services.Announcements;
using Osca.Services.Course;
using Osca.Services.Database;
using Osca.Services.FileArea;
using Osca.Services.FilePath;
using Osca.Services.Food;
using Osca.Services.Osca;
using Osca.Services.RssNews;
using Osca.Services.Sync;

namespace OscaDownloader
{
    class Program
    {
        public class Options
        {
            [Option(
                'u',
                "UserName",
                Required = true,
                HelpText = "Dein OSCA Benutzername")]
            public string UserName { get; set; }

            [Option(
                'p',
                "Password",
                Required = true,
                HelpText = "Dein OSCA Passwort")]
            public string Password { get; set; }

            [Option(
                'c',
                "FedAuthCookie",
                Required = true,
                HelpText = "FedAuth-Cookie vom OscaPortal. Muss im Format 'FedAuth=content' angegeben werden")]
            public string FedAuthCookie { get; set; }

            [Option(
                'o',
                "outputPath",
                Required = true,
                HelpText = "Dort werden alle Dateien abgespeichert")]
            public string OutputPath { get; set; }
        }

        private static bool _hasErrors = false;
        private static Options _options;

        static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseError);

            if (_hasErrors)
            {
                return;
            }

            Console.WriteLine("Starting processes!");

            PathService.OutputPath = _options.OutputPath;

            await DatabaseService.Instance.CreateTables();

            OscaWebService.Instance.Setup(_options.FedAuthCookie);
            var oscaAppService = new OscaAppService();
            await oscaAppService.Login(_options.UserName, _options.Password);

            var syncService = new SyncService(
                    DatabaseService.Instance,
                    oscaAppService,
                    OscaWebService.Instance,
                    new AnnouncementService(),
                    new CourseService(),
                    new CanteenService(),
                    new NewsService(),
                    new FileAreaService(), 
                    new FileService()
                );

            await syncService.SyncAll(SyncProgress, ErrorOccured, ReceivedNewEntities);

        }

        static void ReceivedNewEntities(List<object> obj)
        {
        }


        static void ErrorOccured(List<Exception> obj)
        {
            Console.WriteLine("Es sind Fehler aufgetreten :(");
            Console.WriteLine(string.Join('\n', obj.Select(it => it.ToString())));
        }

        static void SyncProgress(string stepName, int count, int stepCount)
        {
            Console.WriteLine($"Downloade '{stepName}' ({count} von {stepCount})");
        }


        private static void HandleParseError(IEnumerable<Error> errs)
        {
            _hasErrors = true;
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            _options = opts;
        }
    }
}
