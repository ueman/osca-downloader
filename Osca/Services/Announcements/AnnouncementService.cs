using System.Collections.Generic;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.Announcements
{
    public class AnnouncementService
    {
        private readonly SQLiteAsyncConnection _connection;

        public AnnouncementService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public Task LoadAnnouncementsFor(StudentEvent studentEvent, bool show)
        {
            var mapping = new AnnouncementEventMapping
            {
                EventId = studentEvent.CourseID,
                ShowInAnnouncements = show
            };
            return _connection.InsertOrReplaceAsync(mapping);
        }

        public Task<List<AnnouncementEventMapping>> GetEventIdsForAnnouncements()
        {
            return _connection.Table<AnnouncementEventMapping>()
                       .Where(a => a.ShowInAnnouncements == true)
                       .ToListAsync();
        }

        public async Task<List<DisplayAnnouncement>> GetAnnouncementsFromDb()
        {
            var announcements = await _connection.Table<DisplayAnnouncement>()
                              .OrderByDescending(a => a.Created)
                              .ToListAsync();
            return announcements;
        }
    }
}
