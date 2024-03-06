using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using net_iot_data;
using net_iot_data.Data.Repositories.HealthCheck;
using net_iot_data.Data.Repositories.MessageLog;

namespace net_iot_core
{
    /// <summary>
    /// Provides methods for registering core services and creating the database.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers core services with the dependency injection container.
        /// </summary>
        /// <param name="services">The collection of services to add to.</param>
        /// <param name="configuration">The configuration for the application.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            // Add resource registration here
            if (configuration != null)
            {
                services.RegisterDataServices(configuration);
            }
            services.AddTransient<IHealthCheckRepository, HealthCheckRepository>();
            services.AddTransient<IMessageLogRepository, MessageLogRepository>();

            return services;
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static void CreateDatabase(this IServiceProvider serviceProvider)
        {
            // configure the database here
            var serviceScope = serviceProvider.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Create();
        }
    }
}
