using net_iot_core.Services.DeviceProgramming.Bridges;
using net_iot_core.Services.DeviceProgramming.Devices;
using net_iot_core.Services.DeviceProgramming.Exceptions;

namespace net_iot_tests.net_iot_core.DeviceProgramming.Bridges;

[TestClass]
public class UM232H_Tests
{
    [TestMethod]
    public void Blink_UM232H_Bridge()
    {
        // get first connected bridge
        var bridge = (UM232H)Bridge.Open();

        // blink gpio pin
        bool isLedOn = true;
        int count = 1;
        int numBlinks = 15;
        int pinNumber = 4;

        while(count <= numBlinks){
            bridge.WriteToBridgeOnPin(pinNumber, isLedOn);

            isLedOn = !isLedOn;
            count++;

            Thread.Sleep(1000);
        }

        // turn pin low
        bridge.WriteToBridgeOnPin(pinNumber, false);
    }

    [TestMethod]
    public void NotFoundException_Is_Thrown_On_Open()
    {
        Assert.ThrowsException<NotFoundException>(() => (UM232H)Bridge.Open());
    }

    [TestMethod]
    public void AlreadyInitializedException_Is_Thrown_On_Open()
    {
        Assert.ThrowsException<AlreadyInitializedException>(() => 
        {
            var bridge = new UM232H();
            bridge.Open();
            bridge.Open();
        });
    }

    [TestMethod]
    public void BridgeIOException_Is_Thrown_On_Open()
    {
        Assert.ThrowsException<BridgeIOException>(() => 
        {
            var bridge1 = new UM232H();
            bridge1.Open();

            // bridge1.Dispose();

            // opening a second bridge device should result in an IOException type because
            // the first bridges file handle has not been disposed of...
            var bridge2 = new UM232H();
            bridge2.Open();
        });
    }

    [TestMethod]
    public void AlreadyInitializedException_Is_Thrown_On_Spi_Configure()
    {
        Assert.ThrowsException<AlreadyInitializedException>(() => 
        {
            var bridge = new UM232H();
            bridge.Open();
            bridge.ConfigureAsSPIBridge(1, 500000, true, 3);
            bridge.ConfigureAsSPIBridge(1, 500000, true, 3);
        });
    }

    [TestMethod]
    public void Dispose_After_Exception()
    {
        try
        {
            var bridge1 = new UM232H();
            bridge1.Open();

            // try to open a device that does not exist
            var device = Device.Open("DNE", bridge1); // Does Not Exist

            // DISPOSE WILL NEVER GET CALLED!!!
            bridge1.Dispose();
        }
        catch (NotFoundException)
        {
            Assert.ThrowsException<BridgeIOException>(() => 
            {
                var bridge2 = new UM232H();
                bridge2.Open();
            });
        }
    }

    [TestMethod]
    public void Dispose_After_Exception_With_Using_Statements()
    {
        // using statement will call dispose if exception is thrown or not
        // we could also use a finally statement but it's better to implement IDisposable
        // and use a using statement

        // Same example as above but using a using statment now to call dispose
        try
        {
            using (var bridge3 = Bridge.Open())
            {
                // try to open a device that does not exist
                var device = Device.Open("DNE", bridge3); // Does Not Exist
            }
        }
        catch (NotFoundException)
        {
            var bridge4 = new UM232H();
            bridge4.Open();
        }
    }
}
