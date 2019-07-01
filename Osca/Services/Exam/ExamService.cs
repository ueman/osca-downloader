using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.Exam
{
    public class ExamService
    {
        private readonly SQLiteAsyncConnection _connection;

        public ExamService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public async Task<List<IGrouping<string, StudentExam>>> GetExamsFromDb()
        {
            var exams = await _connection.Table<StudentExam>()
                                         .OrderByDescending(e => e.EndDate)
                                         .ToListAsync();

            var groups = exams.GroupBy(e => e.SemesterName)
                              .OrderByDescending(e => e.First().SemesterID);
            return groups.ToList();
        }
    }
}
