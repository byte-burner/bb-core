using net_iot_util;
using net_iot_util.Constants;

namespace net_iot_tests.net_iot_util;

[TestClass]
public class FlashCommandTests
{
    private Application? _app;

    private string _ihxBlinkFilePath = "Source/net_iot_core/Devices/AT89S52_Tests/Executables/Blink.ihx";
    private string _ihxTurnOffFilePath = "Source/net_iot_core/Devices/AT89S52_Tests/Executables/TurnOff.ihx";
    private readonly string _flashSubCommand = "flash";

    [TestInitialize]
    public void Setup()
    {
        _app = CommandRunner.RegisterCommandServices();
    }

    [TestMethod]
    public async Task Flash_Sub_Command_File_Not_Exists_Test()
    {
        int retCode = await CommandRunner.RunWithArgs(
            this._app!.Run,
            new string[]
            {
                CommandRunner.BaseCommand,
                this._flashSubCommand,
                "-d",
                "AT89S52",
                "-h",
                "file_does_not_exist.ihx"
            },
            CommandRunner.OutputFilePath);

        string? errMsg = File.ReadLines(CommandRunner.OutputFilePath).FirstOrDefault();

        Assert.AreEqual(ExitCodes.EXIT_CODE_FAILURE, retCode);
    }

    [TestMethod]
    public async Task Flash_Sub_Command_Required_Option_Blink_Test()
    {
        int retCode = await CommandRunner.RunWithArgs(
            this._app!.Run,
            new string[]
            {
                CommandRunner.BaseCommand,
                this._flashSubCommand,
                "-d",
                "AT89S52",
                "-h",
                this._ihxBlinkFilePath
            },
            CommandRunner.OutputFilePath);

        Assert.AreEqual(ExitCodes.EXIT_CODE_SUCCESS, retCode);
    }

    [TestMethod]
    public async Task Flash_Sub_Command_Required_Option_Turn_Off_Test()
    {
        int retCode = await CommandRunner.RunWithArgs(
            this._app!.Run,
            new string[]
            {
                CommandRunner.BaseCommand,
                this._flashSubCommand,
                "-d",
                "AT89S52",
                "-h",
                this._ihxTurnOffFilePath
            },
            CommandRunner.OutputFilePath);

        Assert.AreEqual(ExitCodes.EXIT_CODE_SUCCESS, retCode);
    }
}



