using net_iot_core.Services.DeviceProgramming;
using net_iot_util.CommandHandlers.Models;
using net_iot_util.Constants;
using net_iot_util.Managers;

namespace net_iot_util.CommandHandlers
{
    public class FlashCommandHandler : ICommandHandler<FlashCommandOptions>
    {
        private readonly IDeviceProgrammingService _deviceProgrammingService;
        
        public FlashCommandHandler(IDeviceProgrammingService deviceProgrammingService)
        {
            this._deviceProgrammingService = deviceProgrammingService;
        }

        #pragma warning disable CS1998
        public async Task<int> Handle(FlashCommandOptions options)
        #pragma warning restore CS1998
        {
            if (options.ConfFileOption != null)
            {
                // parse json config file for hex, bridge, device, etc. also performing validation
                // and utilize same logic below for optional bridges

                // perform flash

                throw new NotImplementedException();
            }
            else if (!options.HexFileOption!.Exists)
            {
                Console.Error.WriteLine(Messages.Error.E001);
                return ExitCodes.EXIT_CODE_FAILURE;
            }
            else{
                if(options.BridgeTypeOption != null){
                    var response = this._deviceProgrammingService.ProgramDevice(
                        options.BridgeTypeOption,
                        options.BridgeSerialNumberOption!,
                        options.DeviceTypeOption!,
                        options.HexFileOption!);

                    return PromptManager.HandleServiceResult(response, Messages.Success.S001);
                }
                else{
                    var response = this._deviceProgrammingService.ProgramDevice(
                        options.DeviceTypeOption!,
                        options.HexFileOption!);

                    return PromptManager.HandleServiceResult(response, Messages.Success.S001);
                }
            }
        }
    }
}
