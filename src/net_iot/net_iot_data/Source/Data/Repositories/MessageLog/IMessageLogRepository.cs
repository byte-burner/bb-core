using net_iot_data.Data.Models;

namespace net_iot_data.Data.Repositories.MessageLog
{
    public interface IMessageLogRepository : IRepository<MessageLogDto>
    {
        Task DeleteAll();
    }
}
