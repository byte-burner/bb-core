using System.CommandLine;
using Microsoft.Extensions.Logging;
using net_iot_util.Commands;

namespace net_iot_util
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly BaseCommand _rootCmd; 

        public Application(
            ILogger<Application> logger,
            BaseCommand rootCmd)
        {
            _logger = logger;
            _rootCmd = rootCmd;
        }

        public async Task<int> Run(string[] args)
        {
            _logger.LogDebug("Successfully started the application");

            var cmd = this._rootCmd.Create();

            return await cmd.InvokeAsync(args);
        }
    }
}

