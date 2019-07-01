using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;
namespace Osca.Services.Course
{
    public class CourseService
    {
        public readonly SQLiteAsyncConnection _connection;

        public CourseService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public async Task<List<IGrouping<string, StudentEvent>>> GetCoursesFromDb()
        {
            var courses = await _connection.Table<StudentEvent>()
                                     .OrderByDescending(c => c.SemesterID)
                                     .OrderBy(c => c.CourseName)
                                     .ToListAsync();

            var grouped = courses.GroupBy(c => c.SemesterName)
                                 .ToList();
            return grouped;
        }

        public Task<List<StudentEvent>> GetCoursesForCurrentSemester()
        {
            var query = @"
select *
from StudentEvent
where SemesterId = (
	select max(SemesterId)
	from StudentEvent
)			
";
            return _connection.QueryAsync<StudentEvent>(query);
        }

        public Task<StudentEvent> GetCourseForId(string courseId)
        {
            return _connection.Table<StudentEvent>()
                .Where(it => it.CourseID == courseId)
                .FirstOrDefaultAsync();
        }
    }
}
