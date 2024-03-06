using net_iot_core.Services.DeviceProgramming.Bridges;
using net_iot_core.Services.DeviceProgramming.Constants;
using net_iot_core.Services.DeviceProgramming.Devices;

namespace net_iot_tests.net_iot_core.DeviceProgramming.Devices.AT89S52_Tests;

[TestClass]
public class AT89S52_Tests
{
    private string blinkProgram = "Source/net_iot_core/DeviceProgramming/Devices/AT89S52_Tests/Executables/Blink.ihx";

    private string turnOffPinProgram = "Source/net_iot_core/DeviceProgramming/Devices/AT89S52_Tests/Executables/TurnOff.ihx";

    private string AT89S52DeviceType = "AT89S52";

    [TestMethod]
    public void Blink_AT89S52_With_UM232H_Bridge_In_Page_Mode()
    {
        // open first connected bridge
        var bridge = (UM232H)Bridge.Open();

        // open device w/ bridge layer transport
        var device = Device.Open(AT89S52DeviceType, bridge);

        // default programming mode for the device is page mode
        device.Program(blinkProgram);
    }

    [TestMethod]
    public void Blink_AT89S52_With_UM232H_Bridge_In_Byte_Mode()
    {
        // open first connected bridge
        var bridge = (UM232H)Bridge.Open();

        // open device w/ bridge layer transport
        var device = Device.Open(AT89S52DeviceType, bridge);

        // set programming mode to byte mode
        device.SetProgrammingMode(ProgrammingMode.BYTE);

        // default programming mode for the device is page mode
        device.Program(blinkProgram);
    }

    [TestMethod]
    public void Turn_Off_Blink_AT89S52_With_UM232H_Bridge_In_Byte_Mode()
    {
        // open first connected bridge
        var bridge = (UM232H)Bridge.Open();

        // open device w/ bridge layer transport
        var device = Device.Open(AT89S52DeviceType, bridge);

        // set programming mode to byte mode
        device.SetProgrammingMode(ProgrammingMode.BYTE);

        // default programming mode for the device is page mode
        device.Program(turnOffPinProgram);
    }

    [TestMethod]
    public void Turn_Off_Blink_AT89S52_With_UM232H_Bridge_In_Page_Mode()
    {
        // open first connected bridge
        var bridge = (UM232H)Bridge.Open();

        // open device w/ bridge layer transport
        var device = Device.Open(AT89S52DeviceType, bridge);

        // default programming mode for the device is page mode
        device.Program(turnOffPinProgram);
    }
}

