using System.CommandLine;

namespace net_iot_util.Commands
{
    public interface ICommand
    {
        Command Create();
    }
}


