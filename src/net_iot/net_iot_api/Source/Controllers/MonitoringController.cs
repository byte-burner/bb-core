using Microsoft.AspNetCore.Mvc;
using net_iot_api.Constants;
using net_iot_core.Services.Monitoring;

namespace net_iot_api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly ILogger<MonitoringController> _logger;
        private readonly IMonitoringService _monitoringService;

        public MonitoringController(ILogger<MonitoringController> logger, IMonitoringService monitoringService)
        {
            this._logger = logger;
            this._monitoringService = monitoringService;
        }

        [HttpGet]
        public ActionResult AddTestLogs()
        {
            try
            {
                _logger.LogWarning("Test warning from logger");
                _logger.LogError(new Exception("Throw fake exception"), "Test error from logger");
                _logger.LogInformation("Test info from logger"); // shouldn't show since the log level is 'Warn'

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Couldn't add test logs"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLogs()
        {
            try
            {
                var response = await this._monitoringService.GetAllLogs();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Couldn't get logs"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAllLogs()
        {
            try
            {
                await this._monitoringService.DeleteAllLogs();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Couldn't delete logs"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

    }
}

