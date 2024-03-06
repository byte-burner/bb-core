using net_iot_data.Data.Models;

namespace net_iot_core.Services.HealthCheck
{
    /// <summary>
    /// Interface for service responsible for managing health check operations.
    /// </summary>
    public interface IHealthCheckService
    {
        /// <summary>
        /// Creates a new health check record.
        /// </summary>
        /// <param name="command">The health check data.</param>
        /// <returns>The created health check data.</returns>
        Task<HealthCheckDto> CreateHealthCheck(HealthCheckDto command);

        /// <summary>
        /// Deletes a health check record.
        /// </summary>
        /// <param name="command">The health check data.</param>
        /// <returns>The deleted health check data.</returns>
        Task<HealthCheckDto> DeleteHealthCheck(HealthCheckDto command);

        /// <summary>
        /// Retrieves all health check records.
        /// </summary>
        /// <returns>A list of health check data.</returns>
        Task<List<HealthCheckDto>> GetAllHealthChecks();

        /// <summary>
        /// Updates a health check record.
        /// </summary>
        /// <param name="command">The health check data.</param>
        /// <returns>The updated health check data.</returns>
        Task<HealthCheckDto?> UpdateHealthCheck(HealthCheckDto command);
    }
}
