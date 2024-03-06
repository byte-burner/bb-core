using Microsoft.EntityFrameworkCore;
using net_iot_data.Data.Models;

namespace net_iot_data.Data.Repositories.MessageLog
{
    public class MessageLogRepository : IMessageLogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageLogRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Task<MessageLogDto> Create(MessageLogDto command)
        {
            throw new NotImplementedException();
        }

        public Task<MessageLogDto> Delete(MessageLogDto command)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAll()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM message_log");
        }

        public Task<MessageLogDto?> Get(MessageLogDto command)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MessageLogDto>> GetAll()
        {
            var records = await this._dbContext.Logs.Select(log => new MessageLogDto()
            {
                Id = log.Id,
                AddedDate = log.AddedDate,
                Level = log.Level,
                Message = log.Message,
                Exception = log.Exception,
            }).ToListAsync();

            return records;
        }

        public Task<MessageLogDto?> Update(MessageLogDto command)
        {
            throw new NotImplementedException();
        }
    }
}