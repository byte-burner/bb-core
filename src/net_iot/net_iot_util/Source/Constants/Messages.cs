using net_iot_core.Shared.ResultHandling.ErrorCodes;

namespace net_iot_util.Constants
{
    public static class Messages
    {
        public static Dictionary<string, string> ServiceErrors = new Dictionary<string, string>()
        {
            {DeviceProgrammingErrorCodes.NotFound, "The bridge or programmable device was not found. To see a list of connected bridge devices run [net_iot_util -l]. To see a list of supported programmable devices run [net_iot_util -s]."},
            {DeviceProgrammingErrorCodes.ProgramFileNotFound, "Cannot find hex file. Make sure it exists."},
            {DeviceProgrammingErrorCodes.AlreadyInitialized, "Device is already initialized for {0}"},
            {DeviceProgrammingErrorCodes.NotInitialized, "Device is not initialized for {0}"},
            {DeviceProgrammingErrorCodes.AlreadyOpen, "Device is already open for {0}"},
            {DeviceProgrammingErrorCodes.ProgramFileExtensionNotSupported, "The program file extension for {0} is not supported"},
            {DeviceProgrammingErrorCodes.ProgramFileFormatIsIncorrect, "Invalid file format for {0}"},
        };

        public static class Error
        {
            public static readonly string E001 = "ERR: Cannot find hex file. Make sure it exists.";
        }

        public static class Success
        {
            public static readonly string S001 = "SUCCESS: Wrote program to device";
        }
    }
}