using System.CommandLine;
using net_iot_core.Services.DeviceProgramming;
using net_iot_util.CommandHandlers;
using net_iot_util.CommandHandlers.Binders;

namespace net_iot_util.Commands
{
    public class FlashCommand : ICommand
    {
        private readonly IDeviceProgrammingService _deviceProgrammingService;
        private readonly FlashCommandHandler _flashHandler;
        
        public FlashCommand(
            IDeviceProgrammingService deviceProgrammingService,
            FlashCommandHandler flashHandler)
        {
            this._deviceProgrammingService = deviceProgrammingService;
            this._flashHandler = flashHandler;
        }

        public Command Create()
        {
            var dOption = new Option<string>(new[] {"-d", "--dev"}, "Specifies device type to flash")
            {
                IsRequired = true, Arity = ArgumentArity.ExactlyOne
            }.FromAmong(this._deviceProgrammingService.SupportedDevices.Select(d => d.Type).ToArray()!);

            var hOption = new Option<FileInfo>(new[] {"-h", "--hex"}, "Specifies the path to a hex file as the main program to be flashed")
            {
                IsRequired = true, Arity = ArgumentArity.ExactlyOne
            };

            var bOption = new Option<string>(new[] {"-b", "--bridge"}, "Depends on -s. Specifies bridge type for flashing")
            {
                Arity = ArgumentArity.ExactlyOne,
            }.FromAmong(this._deviceProgrammingService.SupportedBridges.Select(b => b.Type).ToArray()!);

            var sOption = new Option<string>(new[] {"-s", "--serialNbr"}, "Depends on -b. Specifies the serial number of the device for flashing")
            {
                Arity = ArgumentArity.ExactlyOne,
            };

            var eOption = new Option<bool>(new[] {"-e", "--erase"}, "Perform chip erase before flashing")
            {
                Arity = ArgumentArity.Zero
            };

            var cOption = new Option<FileInfo>(new[] {"-c", "--conf"}, "Specifies a configuration file to be used")
            {
                Arity = ArgumentArity.ExactlyOne,
            };

            var cmd = new Command("flash", "Flashes a hex file onto a device from a bridge device")
            {
                dOption,
                hOption,
                bOption,
                sOption,
                eOption,
                cOption,
            };

            // add sub commands here

            // add command handlers here
            cmd.SetHandler(
                (options) => this._flashHandler.Handle(options),
                new FlashCommandBinder(dOption, hOption, bOption, sOption, eOption, cOption));

            return cmd;
        }
    }
}

