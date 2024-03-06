using net_iot_util.CommandHandlers.Models;

namespace net_iot_util.CommandHandlers
{
    public interface ICommandHandler<T> where T : IOptions
    {
        Task<int> Handle(T options);
    }
}




