using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Osca.Models.Osca;

namespace Osca.Services.Osca
{
    /// <summary>
    /// Zuständig für Anfragen an das Osca-Web-Portal-Backend
    /// </summary>
    public class OscaWebService
    {
        private static readonly Lazy<OscaWebService> lazy =
            new Lazy<OscaWebService>(() => new OscaWebService());

        public static OscaWebService Instance { get { return lazy.Value; } }

        private static readonly string baseAddress = "https://osca.hs-osnabrueck.de";

        /// <summary>
        /// Ein HttpClient für alle Requests, der ist Threadsage 
        /// </summary>
        private HttpClient _client;

        private static HttpClient CreateClient(string fedAuthCookie)
        {

            HttpClient client = new HttpClient();
            // mit dem Cookie authentifiziert sich der Nutzer
            client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", fedAuthCookie);

            // ohne den User-Agent wir kein JavaScript ausgeliefert
            // ohne JavaScript kein JSON in dem die Ankündigung drin steht
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");

            // ohne diesen Header gibt es XML zurück, aber das ist sooo scheiße
            // das JSON ist besser aber auch immer noch nicht "schön" und mit Metadaten verseucht
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json;odata=verbose");
            return client;
        }

        public void Setup(string fedAuthCookie)
        {
            _client = CreateClient(fedAuthCookie);
        }

        public Task<List<Announcement>> GetAnouncementsForCourses(List<StudentEvent> courses, CancellationToken cancellationToken = default)
        {
            return GetAnouncementsForCourses(courses.Select(c => c.CourseID), cancellationToken);
        }

        public Task<List<Announcement>> GetAnouncementsForCourses(List<AnnouncementEventMapping> courses, CancellationToken cancellationToken = default)
        {
            return GetAnouncementsForCourses(courses.Select(c => c.EventId), cancellationToken);
        }

        private async Task<List<Announcement>> GetAnouncementsForCourses(IEnumerable<string> courses, CancellationToken cancellationToken = default)
        {
            var tasks = courses.Select(c => GetAnouncements(c, cancellationToken)).ToList();

            // dies kann sehr schön parallelisiert werden
            var announcements = await Task.WhenAll(tasks);

            return announcements
                .SelectMany(a => a)
                .OrderByDescending(a => a.Created)
                .ToList();
        }

        /// <summary>
        /// Liefert die Announcements einer Veranstaltung zurück.
        /// </summary>
        /// <returns>The anouncements.</returns>
        /// <param name="studentEvent">Student event.</param>
        public Task<List<Announcement>> GetAnouncements(StudentEvent studentEvent, CancellationToken cancellationToken = default)
        {
            return GetAnouncements(studentEvent.CourseID, cancellationToken);
        }

        private async Task<List<Announcement>> GetAnouncements(string courseId, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync(AnnouncementsApiEndpoint(courseId), cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            
            if (!StartsJsonLike(content))
            {
                Console.WriteLine($"Error Invalid Response: {content}");
            }
            
            var alist = JsonConvert.DeserializeObject<AnnouncementRootObject>(content);
            return alist.ListObject.Results
                        .AsParallel()
                        .Select(a => ToAnnouncement(a, courseId))
                        .ToList();
        }

        private Announcement ToAnnouncement(AnnouncementResult result, string courseId)
        {
            return new Announcement
            {
                Id = result.GUID,
                Title = result.Title,
                Body = result.Body,
                Created = result.Created,
                Modified = result.Modified,
                AnnouncementId = result.Id,
                CourseId = courseId
            };
        }

        private string AnnouncementsApiEndpoint(string courseId)
            => $"{baseAddress}/lms/{courseId}/_api/web/lists/getByTitle('Ank%C3%BCndigungen')/items";

        public async Task<List<CourseFile>> GetFilesForCourses(List<StudentEvent> courses, CancellationToken cancellationToken = default)
        {
            var tasks = courses.Select(c => GetFilesForCourse(c.CourseID, cancellationToken));
            var files = await Task.WhenAll(tasks);
            return files
                .AsParallel()
                .Where(f => f != null)
                .SelectMany(f => f)
                .ToList();
        }

        public async Task<List<CourseFile>> GetFilesForCourse(string courseId, CancellationToken cancellationToken = default)
        {
            var fileApiEndpoint = FileApiEndpoint(courseId);
            var response = await _client.GetAsync(fileApiEndpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            if (!StartsJsonLike(content))
            {
                Console.WriteLine($"Error Invalid Response: {content}");
            }
            var rootObject = JsonConvert.DeserializeObject<FileListRootObject>(content);

            var fileDownloadTasks = rootObject.FileList.Results
                                         .AsParallel()
                                         // FileSystemObjectType heißt es ist eine Datei
                                         .Where(f => f.FileSystemObjectType == 0)
                                         .Select(f => LoadFile(_client, f.File.FileUrl.Url, cancellationToken));

            var files = await Task.WhenAll(fileDownloadTasks);

            return files
                .AsParallel()
                .Where(f => f != null)
                .Select(f => ToCourseFile(f, courseId))
                .ToList();
        }

        private async Task<SharepointFile> LoadFile(HttpClient client, string url, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(url, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            var rootObject = JsonConvert.DeserializeObject<FileRootObject>(content);
            return rootObject.File;
        }

        private CourseFile ToCourseFile(SharepointFile file, string courseId)
        {
            return new CourseFile
            {
                Name = file.Name,
                ServerRelativeUrl = file.ServerRelativeUrl,
                Created = file.TimeCreated,
                LastModified = file.TimeLastModified,
                CourseId = courseId
            };
        }

        /// <summary>
        /// Gibt alle eine Liste mit allen Dateien zurück.
        /// Ordner wurden rausgefiltert, siehe auch
        /// https://sharepoint.stackexchange.com/questions/124846/rest-with-filter-for-list-items-in-sharepoint-2013
        /// </summary>
        /// <param name="courseId">Course identifier.</param>
        private string FileApiEndpoint(string courseId)
            => $"{baseAddress}/lms/{courseId}/_api/web/lists/getByTitle('Dateibereich')/items?$filter=startswith(ContentTypeId, '0x0101')";

        /// <summary>
        /// Lädt eine Datei herunter und speichert sie an dem gegebenem Pfad unter dem 
        /// übergebenem Namen.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="destination">Destination.</param>
        public async Task DowloadFile(string source, string destination, string fileName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException(nameof(destination));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(nameof(fileName));
            }

            var response = await _client.GetAsync(source);

            using (var fileStream = await response.Content.ReadAsStreamAsync())
            {
                Directory.CreateDirectory(destination);
                using (var file = File.Create(Path.Combine(destination, fileName)))
                {
                    fileStream.CopyTo(file);
                }
            }
        }

        private bool StartsJsonLike(string content)
        {
            return content.StartsWith("{") || content.StartsWith("[");
        }
    }
}
