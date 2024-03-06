using System.CommandLine;
using net_iot_util.CommandHandlers;
using net_iot_util.CommandHandlers.Binders;

namespace net_iot_util.Commands
{
    public class BaseCommand : ICommand
    {
        private readonly BaseCommandHandler _baseHandler;
        private readonly FlashCommand _flashCommand;

        public BaseCommand(
            BaseCommandHandler baseHandler,
            FlashCommand flashCommand)
        {
            this._baseHandler = baseHandler;
            this._flashCommand = flashCommand;
        }

        public Command Create()
        {
            var lOption = new Option<bool>(new[] {"-l", "--list"}, "List bridges connected to machine")
                { Arity = ArgumentArity.Zero };

            var sOption = new Option<bool>(new[] {"-s", "--supported"}, "List of supported devices to flash")
                { Arity = ArgumentArity.Zero };

            var cmd = new RootCommand("Prints information about devices and bridges")
            {
                lOption,
                sOption,
            };

            // add sub commands here
            cmd.AddCommand(_flashCommand.Create());

            // add command handler here
            cmd.SetHandler(
                (options) => this._baseHandler.Handle(options),
                new BaseCommandBinder(lOption, sOption));

            return cmd;
        }
    }
}
