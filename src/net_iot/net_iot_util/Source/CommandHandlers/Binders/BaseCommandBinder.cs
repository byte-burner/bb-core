using System.CommandLine;
using System.CommandLine.Binding;
using net_iot_util.CommandHandlers.Models;

namespace net_iot_util.CommandHandlers.Binders
{
    public class BaseCommandBinder : BinderBase<BaseCommandOptions>
    {
        private readonly Option<bool> _listOption;
        private readonly Option<bool> _supportOption;

        public BaseCommandBinder(
            Option<bool> listOption,
            Option<bool> supportOption)
        {
            _listOption = listOption;
            _supportOption = supportOption;
        }

        protected override BaseCommandOptions GetBoundValue(BindingContext bindingContext) =>
            new BaseCommandOptions
            {
                ListOption = bindingContext.ParseResult.GetValueForOption(_listOption),
                SupportOption = bindingContext.ParseResult.GetValueForOption(_supportOption),
            };
    }
}

