using Microsoft.AspNetCore.Mvc;
using net_iot_api.Constants;
using net_iot_core.Services.HealthCheck;
using net_iot_data.Data.Models;

namespace net_iot_api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;

        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckController(ILogger<HealthCheckController> logger, IHealthCheckService healthCheckService)
        {
            this._logger = logger;
            this._healthCheckService = healthCheckService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateHealthCheck(HealthCheckDto command)
        {
            try
            {
                var response = await this._healthCheckService.CreateHealthCheck(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create health check item with key, {healthCheckKey}",
                    command!.HealthCheckKey
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateHealthCheck(HealthCheckDto command)
        {
            try
            {
                var response = await this._healthCheckService.UpdateHealthCheck(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update health check item with key, {healthCheckKey}",
                    command!.HealthCheckKey
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteHealthCheck(HealthCheckDto command)
        {
            try
            {
                var response = await this._healthCheckService.DeleteHealthCheck(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to delete health check item with key, {healthCheckKey}",
                    command!.HealthCheckKey
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllHealthChecks()
        {
            try
            {
                var response = await this._healthCheckService.GetAllHealthChecks();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get all health checks"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpGet]
        public ActionResult Ping()
        {
            return Ok();
        }
    }
}
