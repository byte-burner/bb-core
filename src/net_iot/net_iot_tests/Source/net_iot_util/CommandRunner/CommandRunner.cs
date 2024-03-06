using Microsoft.Extensions.DependencyInjection;
using net_iot_core;
using net_iot_util;

namespace net_iot_tests.net_iot_util;

public static class CommandRunner
{
    public static string OutputFilePath = "Source/Output.txt";

    public static string BaseCommand = "testhost";

    public static async Task<int> RunWithArgs(Func<string[], Task<int>> cmdFn, string[] args, string outputFilePath)
    {
        int retCode = -1;

        // Create a StringWriter to capture the output
        using (StringWriter sw = new StringWriter())
        {
            // Redirect the standard output and standard error to the StringWriter
            Console.SetOut(sw);
            Console.SetError(sw);

            // run app w/ args
            retCode = await cmdFn(args);

            // Get the captured output
            string capturedOutput = sw.ToString();

            // Write the string content to the file
            File.WriteAllText(outputFilePath, capturedOutput);

            // Restore the standard output
            Console.SetOut(Console.Out);
        }

        return retCode;
    }

    public static Application RegisterCommandServices()
    {
        var serviceProvider = new ServiceCollection()
            .RegisterCoreServices()
            .RegisterUtilServices()
            .AddLogging()
            .BuildServiceProvider();

        return serviceProvider.GetRequiredService<Application>();
    }
}