using System.CommandLine;
using System.CommandLine.Binding;
using net_iot_util.CommandHandlers.Models;

namespace net_iot_util.CommandHandlers.Binders
{
    public class FlashCommandBinder : BinderBase<FlashCommandOptions>
    {
        private readonly Option<string> _deviceTypeOption;
        private readonly Option<FileInfo> _hexFileOption;
        private readonly Option<string> _bridgeTypeOption;
        private readonly Option<string> _bridgeSerialNbrOption;
        private readonly Option<bool> _eraseOption;
        private readonly Option<FileInfo> _confFileOption;

        public FlashCommandBinder(
            Option<string> deviceTypeOption,
            Option<FileInfo> hexFileOption,
            Option<string> bridgeTypeOption,
            Option<string> bridgeSerialNbrOption,
            Option<bool> eraseOption,
            Option<FileInfo> confFileOption)
        {
            _deviceTypeOption = deviceTypeOption;
            _hexFileOption = hexFileOption;
            _bridgeTypeOption = bridgeTypeOption;
            _bridgeSerialNbrOption = bridgeSerialNbrOption;
            _eraseOption = eraseOption;
            _confFileOption = confFileOption;
        }

        protected override FlashCommandOptions GetBoundValue(BindingContext bindingContext) =>
            new FlashCommandOptions
            {
                DeviceTypeOption = bindingContext.ParseResult.GetValueForOption(_deviceTypeOption),
                HexFileOption = bindingContext.ParseResult.GetValueForOption(_hexFileOption),
                BridgeTypeOption = bindingContext.ParseResult.GetValueForOption(_bridgeTypeOption),
                BridgeSerialNumberOption = bindingContext.ParseResult.GetValueForOption(_bridgeSerialNbrOption),
                EraseOption = bindingContext.ParseResult.GetValueForOption(_eraseOption),
                ConfFileOption = bindingContext.ParseResult.GetValueForOption(_confFileOption),
            };
    }
}
