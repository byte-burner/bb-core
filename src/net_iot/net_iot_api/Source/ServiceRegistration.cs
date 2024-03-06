using net_iot_core.Services.DeviceProgramming;
using net_iot_core.Services.HealthCheck;
using net_iot_core.Services.Monitoring;

namespace net_iot_api
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterApiServices(this IServiceCollection services)
        {
            // Add resource registration here
            services.AddTransient<IHealthCheckService, HealthCheckService>();
            services.AddTransient<IDeviceProgrammingService, DeviceProgrammingService>();
            services.AddTransient<IMonitoringService, MonitoringService>();

            return services;
        }
    }
}

