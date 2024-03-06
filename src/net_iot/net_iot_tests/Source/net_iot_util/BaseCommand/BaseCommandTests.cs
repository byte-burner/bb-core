using net_iot_util;
using net_iot_util.Constants;

namespace net_iot_tests.net_iot_util;

[TestClass]
public class BaseCommandTests
{
    private Application? _app;

    [TestInitialize]
    public void Setup()
    {
        _app = CommandRunner.RegisterCommandServices();
    }

    [TestMethod]
    public async Task Base_Command_List_Option_Test()
    {
        int retCode = await CommandRunner.RunWithArgs(
            this._app!.Run,
            new string[]
            {
                CommandRunner.BaseCommand,
                "-l"
            },
            CommandRunner.OutputFilePath);

        Assert.AreEqual(ExitCodes.EXIT_CODE_SUCCESS, retCode);
    }

    [TestMethod]
    public async Task Base_Command_Supported_Option_Test()
    {
        int retCode = await CommandRunner.RunWithArgs(
            this._app!.Run,
            new string[]
            {
                CommandRunner.BaseCommand,
                "-s"
            },
            CommandRunner.OutputFilePath);

        Assert.AreEqual(ExitCodes.EXIT_CODE_SUCCESS, retCode);
    }
}



