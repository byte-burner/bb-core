using net_iot_data.Data.Models;
using net_iot_data.Data.Repositories.HealthCheck;

namespace net_iot_core.Services.HealthCheck
{
    /// <summary>
    /// Service for managing health check operations.
    /// </summary>
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IHealthCheckRepository _healthCheckRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckService"/> class.
        /// </summary>
        /// <param name="healthCheckRepository">The health check repository.</param>
        public HealthCheckService(IHealthCheckRepository healthCheckRepository)
        {
            this._healthCheckRepository = healthCheckRepository;
        }

        /// <inheritdoc/>
        public async Task<HealthCheckDto> CreateHealthCheck(HealthCheckDto command)
        {
            return await this._healthCheckRepository.Create(command);
        }

        /// <inheritdoc/>
        public async Task<HealthCheckDto> DeleteHealthCheck(HealthCheckDto command)
        {
            return await this._healthCheckRepository.Delete(command);
        }

        /// <inheritdoc/>
        public async Task<List<HealthCheckDto>> GetAllHealthChecks()
        {
            return await this._healthCheckRepository.GetAll();
        }

        /// <inheritdoc/>
        public async Task<HealthCheckDto?> UpdateHealthCheck(HealthCheckDto command)
        {
            return await this._healthCheckRepository.Update(command);
        }
    }
}