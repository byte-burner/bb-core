using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using net_iot_api.Constants;
using net_iot_api.Extensions;
using net_iot_api.Hubs;
using net_iot_api.Models;
using net_iot_core.Services.DeviceProgramming;
using net_iot_core.Services.DeviceProgramming.Models;

namespace net_iot_api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProgrammingController : ControllerBase
    {
        private readonly ILogger<ProgrammingController> _logger;
        private readonly IDeviceProgrammingService _deviceProgrammingService;
        private readonly IHubContext<MonitorDeviceHub> _hubContext;

        public ProgrammingController(
            ILogger<ProgrammingController> logger,
            IDeviceProgrammingService deviceProgrammingService,
            IHubContext<MonitorDeviceHub> hubContext)
        {
            this._logger = logger;
            this._deviceProgrammingService = deviceProgrammingService;
            this._hubContext = hubContext;
        }

        [HttpGet]
        public void StartMonitoringBridgeEvents()
        {
            _logger.LogInformation("Starting bridge monitoring service");

            _deviceProgrammingService.StartMonitoringBridgeEvents((IEnumerable<BridgeInfo> bridgeInfo) =>
            {
                _hubContext.Clients.All.SendAsync("ReceiveBridgeInfo", bridgeInfo.Select(b => new BridgeDto()
                {
                    BridgeType = b.Type,
                    BridgeSerialNbr = b.SerialNbr,
                    BridgeDescription = $"{b.Type} - {b.SerialNbr}",
                }));
            });
        }

        [HttpGet]
        public ActionResult GetAllSupportedDevices()
        {
            return Ok(this._deviceProgrammingService.SupportedDevices.Select((d) => new DeviceDto()
            {
                DeviceType = d.Type,
            }));
        }
        
        [HttpGet]
        public ActionResult GetAllConnectedBridges()
        {
            try
            {
                var response = this._deviceProgrammingService.GetAllConnectedBridges();

                return response.IsSuccess ? Ok(response.Payload) : BadRequest(response.ToProblemDetails());
            } 
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Could not get bridge info"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpGet]
        public ActionResult GetSupportedFileExtensionsByDeviceType(string type)
        {
            try
            {
                var response = this._deviceProgrammingService.GetSupportedFileExtensionsByDeviceType(type);

                return response.IsSuccess ? Ok(response.Payload) : BadRequest(response.ToProblemDetails());
            } 
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Could not get bridge info"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }

        [HttpPost]
        public ActionResult ProgramDevice(ProgrammingInfo info)
        {
            try
            {
                var response = this._deviceProgrammingService.ProgramDevice(
                    info.BridgeType!,
                    info.BridgeSerialNbr!,
                    info.DeviceType!,
                    info.ProgramFilePath!);

                return response.IsSuccess ? Ok() : BadRequest(response.ToProblemDetails());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Could not successfully program the device"
                );
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Messages.Error.InternalServerError);
        }
    }
}