using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.Appointments
{
    public class AppointmentService
    {
        private readonly SQLiteAsyncConnection _connection;

        public AppointmentService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public async Task<List<IGrouping<string, Appointment>>> GetAppointmentsFromDb()
        {
            var appointments = await _connection.Table<Appointment>()
                                          .Where(a => a.StartDate > DateTime.Now)
                                          .OrderBy(a => a.StartDate)
                                          .ToListAsync();
            return appointments.GroupBy(a => a.FormattedDay).ToList();
        }
    }
}
