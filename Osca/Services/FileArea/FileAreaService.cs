using System.Collections.Generic;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.FileArea
{
    public class FileAreaService
    {
        private readonly SQLiteAsyncConnection _connection;

        public FileAreaService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public Task<List<CourseFile>> GetFilesForCourse(StudentEvent course)
        {
            return _connection.Table<CourseFile>()
                       .Where(f => f.CourseId == course.CourseID)
                       .ToListAsync();
        }
        
        public async Task<List<CourseFile>> GetFilesForCourses(List<StudentEvent> courses)
        {
            // yolo performance
            var list = new List<CourseFile>();
            foreach (var course in courses)
            {
                list.AddRange(await GetFilesForCourse(course));
            }
            return list;
        }
    }
}
