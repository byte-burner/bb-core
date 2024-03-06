
using net_iot_core.Services.FileHandling.Parsers;

namespace net_iot_tests.net_iot_core.FileHandling.Parsers;

[TestClass]
public class Factory_Tests
{
    private string ihxBlinkFilePath = "Source/net_iot_core/DeviceProgramming/Devices/AT89S52_Tests/Executables/Blink.ihx";

    [TestMethod]
    public void Parse_Memory_Entry_With_IntelHexFileParser()
    {
        IHexFileParser parser =  new IntelHexFileParser(ihxBlinkFilePath);

        parser.Parse((memoryEntries) =>
        {
            foreach (var entry in memoryEntries)
            {
                
            }
        }, 8192); // max num bytes allowable is 8K
    }

    [TestMethod]
    public void Parse_Page_Entry_With_IntelHexFileParser()
    {
        IHexFileParser parser =  new IntelHexFileParser(ihxBlinkFilePath);

        parser.Parse((pageEntry) =>
        {
        }, 256, 8);
    }
}


