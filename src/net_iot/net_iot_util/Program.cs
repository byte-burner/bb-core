using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using net_iot_core;
using net_iot_core.Shared;
using net_iot_util.Constants;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace net_iot_util
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                /**
                * CONFIGURE BASE PATH
                */
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string baseSettingsPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Settings";

                /**
                * CONFIGURE APP HOST
                */
                var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings()
                {
                    DisableDefaults = true, // OPTIMIZATION: when setting to true the app builder
                                            // will not search all paths for default configs and files. Having
                                            // this set to the default of false will slow our program down
                                            // tremendously! DO NOT SET TO FALSE
                });

                builder.Configuration
                    .SetBasePath(baseSettingsPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                /**
                * CONFIGURE NLOG
                */
                var logger = NLog.LogManager
                            .Setup()
                            .LoadConfigurationFromAppSettings(baseSettingsPath)
                            .GetCurrentClassLogger();

                if (logger == null)
                {
                    throw new NLogConfigurationException("Could not configure NLog. Check your configuration settings to continue");
                }

                /**
                * CONFIGURE THE SERVICES
                */

                // add service options here
                // builder.Services.AddOptions<DeviceOptions>().Bind(builder.Configuration.GetSection("Device"));

                // add services here
                builder.Services.RegisterCoreServices(builder.Configuration);
                builder.Services.RegisterUtilServices();
                builder.Services.AddLogging(log => 
                {
                    log.ClearProviders();
                    log.AddNLog();
                });

                /**
                * CONFIGURE THE ERROR MANAGER TO USE OUR CUSTOM ERRORS
                */
                ErrorManager.Load(Messages.ServiceErrors);

                /**
                * START THE APPLICATION HOST
                */
                var host = builder.Build();
                var provider = host.Services.CreateScope();
                var app = provider.ServiceProvider.GetRequiredService<Application>();

                logger.Info("Starting the application...");

                return await app.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ExitCodes.EXIT_CODE_FAILURE;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}

