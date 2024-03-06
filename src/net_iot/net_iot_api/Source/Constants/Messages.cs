using Microsoft.AspNetCore.Mvc;
using net_iot_core.Shared.ResultHandling.ErrorCodes;

namespace net_iot_api.Constants
{
    public static class Messages
    {
        public static Dictionary<string, string> ServiceErrors = new Dictionary<string, string>()
        {
            {DeviceProgrammingErrorCodes.NotFound, "Device could not be found for {0} because it isn't a supported type"},
            {DeviceProgrammingErrorCodes.AlreadyInitialized, "Device is already initialized for {0}"},
            {DeviceProgrammingErrorCodes.NotInitialized, "Device is not initialized for {0}"},
            {DeviceProgrammingErrorCodes.AlreadyOpen, "Device is already open for {0}"},
            {DeviceProgrammingErrorCodes.ProgramFileNotFound, "The program file was not found at location {0}"},
            {DeviceProgrammingErrorCodes.ProgramFileExtensionNotSupported, "The program file extension for {0} is not supported"},
            {DeviceProgrammingErrorCodes.ProgramFileFormatIsIncorrect, "Invalid file format for {0}"},
        };

        public static class Error
        {
            public static readonly ProblemDetails InternalServerError = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7807",
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", new[] { new net_iot_core.Shared.ResultHandling.Error("InternalServerError", "An internal server error has occurred") }},
                }
            };
        }
    }
}