using System.Collections.Generic;
using System.Threading.Tasks;
using Osca.Models.Osca;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.Messages
{
    public class MessagesService
    {
        public SQLiteAsyncConnection _connection;

        public MessagesService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public async Task<List<Message>> GetMessagesFromDb()
        {
            var messages = await _connection.Table<Message>()
                                      .OrderByDescending(m => m.Date)
                                      .ToListAsync();
            return messages;
        }
    }
}
