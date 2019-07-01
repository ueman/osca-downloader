using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Osca;
using Osca.Services.FilePath;

namespace Osca.Services.FileArea
{
    public class FileService
    {
        private readonly OscaWebService _oscaWebService;

        public FileService()
        {
            _oscaWebService = OscaWebService.Instance;
        }

        public Task DownloadFile(CourseFile file, StudentEvent studentEvent)
        {
            var path = file.ServerRelativeUrl.Replace(Path.GetFileName(file.ServerRelativeUrl), "");
            
            // Path.Combine(,) ist komisch wenn der zweite Pfad mit einem PathSeperator anfängt :/
            // siehe dazu https://docs.microsoft.com/de-de/dotnet/api/system.io.path.combine?view=netframework-4.7.2
            path = path.Substring(1);
            
            path = Path.Combine(studentEvent.CourseName, path);
            var destination = Path.Combine(GetFileStoragePath(), path);
            return _oscaWebService.DowloadFile(file.DownloadUrl, destination, Path.GetFileName(file.ServerRelativeUrl));
        }

        private static string GetFileStoragePath()
        {
            var path = Path.Combine(PathService.OutputPath, "files");
            Debug.WriteLine($"File storage path: {path}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
