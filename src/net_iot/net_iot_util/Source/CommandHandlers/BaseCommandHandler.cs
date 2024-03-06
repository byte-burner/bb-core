using net_iot_api.Models.Dto;
using net_iot_core.Services.DeviceProgramming;
using net_iot_util.CommandHandlers.Models;
using net_iot_util.Constants;
using net_iot_util.Managers;

namespace net_iot_util.CommandHandlers
{
    public class BaseCommandHandler : ICommandHandler<BaseCommandOptions>
    {
        private readonly IDeviceProgrammingService _deviceProgrammingService;
        
        public BaseCommandHandler(IDeviceProgrammingService deviceProgrammingService)
        {
            this._deviceProgrammingService = deviceProgrammingService;
        }

        #pragma warning disable CS1998
        public async Task<int> Handle(BaseCommandOptions options)
        #pragma warning restore CS1998
        {
            if (options.ListOption)
            {
                // print out list of connected bridge devices
                var response = this._deviceProgrammingService.GetAllConnectedBridges();

                PromptManager.PrintComplexTypeList<BridgeDto>(response.Payload?.Select(b => new BridgeDto()
                {
                    BridgeType = b.Type,
                    BridgeSerialNbr = b.SerialNbr,
                })!);
            }
            
            if (options.SupportOption)
            {
                // print out list of supported devices to flash
                var devices = this._deviceProgrammingService.SupportedDevices;
                PromptManager.PrintComplexTypeList<DeviceDto>(devices.Select(d => new DeviceDto()
                {
                    DeviceType = d.Type
                }));
            }

            return ExitCodes.EXIT_CODE_SUCCESS;
        }
    }
}



