
using net_iot_data.Data.Models;

namespace net_iot_core.Services.Monitoring
{
    /// <summary>
    /// Interface for a service responsible for monitoring message logs.
    /// </summary>
    public interface IMonitoringService
    {
        /// <summary>
        /// Deletes all message logs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAllLogs();

        /// <summary>
        /// Retrieves all message logs.
        /// </summary>
        /// <returns>A list of message logs.</returns>
        Task<List<MessageLogDto>> GetAllLogs();
    }
}

