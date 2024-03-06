using net_iot_api.Constants;
using net_iot_api.Extensions;
using net_iot_api.Middleware;
using net_iot_core;
using net_iot_core.Shared;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace net_iot_api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                /**
                * CONFIGURE BASE PATH
                */
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string baseSettingsPath = $"{AppDomain.CurrentDomain.BaseDirectory}Settings/";

                Console.WriteLine($"Starting program with base path at, ${basePath}");
                Console.WriteLine($"Starting program with configuration base path at, ${baseSettingsPath}");

                /**
                * CONFIGURE APP HOST
                */
                var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions()
                {
                    ContentRootPath = basePath, // OPTIMIZATION: when setting to our base path the builder
                                                // will not search all paths for default configs and files. Not
                                                // setting this to the base path will slow our program down
                                                // tremendously! DO NOT CHANGE, UNLESS FOR A VERY GOOD REASON
                    Args = args
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

                // Add services to the container.
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddControllers();
                builder.Services.AddSignalR();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // add services here
                builder.Services.RegisterCoreServices(builder.Configuration);
                builder.Services.RegisterApiServices();
                builder.Services.AddLogging(log => 
                {
                    log.ClearProviders();
                    log.AddNLog();
                });
                builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
                builder.Services.AddProblemDetails();

                /**
                * CONFIGURE THE ERROR MANAGER TO USE OUR CUSTOM ERRORS
                */
                ErrorManager.Load(Messages.ServiceErrors);

                /**
                * START THE APPLICATION HOST
                */
                var app = builder.Build();

                // ensure that the database is created
                app.Services.CreateDatabase();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseAuthorization();
                app.MapControllers();
                app.MapHubs();
                app.UseCors(policy => policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // Fixes InvalidOperationException => 
                                                        // only way to allow all origins w/ allow credentials also enabled
                    .AllowCredentials());
                app.UseExceptionHandler();

                logger.Info("Starting the application...");

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Shutting down the application and log manager...");

                NLog.LogManager.Shutdown();
            }
        }
    }
}