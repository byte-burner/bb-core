using net_iot_api.Hubs;

namespace net_iot_api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MapHubs(this WebApplication app)
        {
            // add more SignalR hubs here
            app.MapHub<MonitorDeviceHub>("/MonitorDeviceHub");
        }
    }
}
