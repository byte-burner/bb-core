using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using net_iot_data.Options;

namespace net_iot_data
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            // add service options here
            services.AddOptions<DatabaseOptions>().Bind(configuration.GetSection("Database"));

            // add service registration here
            services.AddDbContext<ApplicationDbContext>();

            return services;
        }
    }
}
