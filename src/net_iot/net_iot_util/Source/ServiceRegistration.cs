using Microsoft.Extensions.DependencyInjection;
using net_iot_core.Services.DeviceProgramming;
using net_iot_util.CommandHandlers;
using net_iot_util.Commands;

namespace net_iot_util
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterUtilServices(this IServiceCollection services)
        {
            // Add resource registration here
            services.AddTransient<IDeviceProgrammingService, DeviceProgrammingService>();
            services.AddTransient<FlashCommandHandler>();
            services.AddTransient<BaseCommandHandler>();
            services.AddTransient<FlashCommand>();
            services.AddTransient<BaseCommand>();
            services.AddSingleton<Application>();

            return services;
        }
    }
}
