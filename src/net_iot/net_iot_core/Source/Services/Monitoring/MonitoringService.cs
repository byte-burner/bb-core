using net_iot_data.Data.Models;
using net_iot_data.Data.Repositories.MessageLog;

namespace net_iot_core.Services.Monitoring
{
    /// <summary>
    /// Service for monitoring message logs.
    /// </summary>
    public class MonitoringService : IMonitoringService
    {
        private readonly IMessageLogRepository _loggingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoringService"/> class.
        /// </summary>
        /// <param name="loggingRepository">The message log repository.</param>
        public MonitoringService(IMessageLogRepository loggingRepository)
        {
            this._loggingRepository = loggingRepository;
        }

        /// <inheritdoc/>
        public async Task DeleteAllLogs()
        {
            await this._loggingRepository.DeleteAll();
        }

        /// <inheritdoc/>
        public async Task<List<MessageLogDto>> GetAllLogs()
        {
            return await this._loggingRepository.GetAll();
        }
    }
}
