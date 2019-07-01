using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Osca.Extensions;
using Osca.Models.Canteens;
using Osca.Models.News;
using Osca.Models.Osca;
using SQLite;
using Osca.Services.FilePath;

namespace Osca.Services.Database
{
    public class DatabaseService
    {

        private static readonly Lazy<DatabaseService> lazy =
            new Lazy<DatabaseService>(() => new DatabaseService());

        public static DatabaseService Instance { get { return lazy.Value; } }

        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService()
        {
            _connection = new SQLiteAsyncConnection(GetDatabasePath());
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _connection;
        }

        public static string GetDatabasePath()
        {
            var path = Path.Combine(PathService.OutputPath, "osca.db");
            Console.WriteLine($"Database path: {path}");
            return path;
        }

        public async Task CreateTables()
        {
            await _connection.CreateTableAsync<Canteen>().AnyContext();
            await _connection.CreateTableAsync<Dish>().AnyContext();
            await _connection.CreateTableAsync<StudentExam>().AnyContext();
            await _connection.CreateTableAsync<Message>().AnyContext();
            await _connection.CreateTableAsync<StudentEvent>().AnyContext();
            await _connection.CreateTableAsync<Appointment>().AnyContext();
            await _connection.CreateTableAsync<Announcement>().AnyContext();
            await _connection.CreateTableAsync<AnnouncementEventMapping>().AnyContext();
            await _connection.ExecuteAsync(Semester.CreateSemesterView()).AnyContext();
            await _connection.ExecuteAsync(DisplayAnnouncement.CreateView).AnyContext();
            await _connection.CreateTableAsync<CourseFile>().AnyContext();
            await _connection.CreateTableAsync<RssItem>().AnyContext();
        }

        public async Task DropTableAndInsertAll<T>(IEnumerable<T> objects) where T : new()
        {
            // Löschen und einfügen in einer Transaktion,
            // damit man nicht ausversehen nur die Daten löscht
            await _connection.RunInTransactionAsync(connection =>
            {
                connection.DeleteAll<T>();
                connection.InsertAll(objects: objects, runInTransaction: false);
            }).AnyContext();
        }
    }
}
