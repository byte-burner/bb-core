using Microsoft.AspNetCore.SignalR;

namespace net_iot_api.Hubs
{
    public class MonitorDeviceHub : Hub
    {
        private readonly ILogger<MonitorDeviceHub> _logger;

        public MonitorDeviceHub(ILogger<MonitorDeviceHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            // custom functionality to execute when connection is established
            _logger.LogInformation("Connecting to SignalR MonitorDeviceHub");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // custom functionality to execute when connection is disconnected
            _logger.LogInformation("Disconnecting from SignalR MonitorDeviceHub");

            return base.OnDisconnectedAsync(exception);
        }
    }
}