using net_iot_core.Services.DeviceProgramming.Bridges;
using net_iot_core.Services.DeviceProgramming.Constants;
using net_iot_core.Services.DeviceProgramming.Exceptions;
using net_iot_core.Services.DeviceProgramming.Models;

namespace net_iot_core.Services.DeviceProgramming.Devices
{
    /// <summary>
    /// Provides the functionality to work with programmable devices.
    /// </summary>
    public static class Device
    {
        private static List<DeviceInfo> _supportedDevices = new List<DeviceInfo>()
        {
            new DeviceInfo() { Type = "AT89S51" }, // NOTE: Remove this test device in production
            new DeviceInfo() { Type = "AT89S52" },

            // add more supported bridge devices here
        };

        public static List<DeviceInfo> SupportedDevices { get { return _supportedDevices ; } }

        /// <summary>
        /// Gets the programmable file extensions for the device
        /// </summary>
        /// <param name="type">The type of programmable device</param>
        /// <returns>An array of file extensions that are supported by the device for programming</returns>
        public static string[] GetSupportedFileExtensionsByDeviceType(string type)
        {
            switch (type)
            {
                case SupportedDeviceType.AT89S52:
                    return AT89S52.SupportedFileExtensions;
                
                default:
                    break;
            }

            throw new NotFoundException("The device was not found");
        }

        /// <summary>
        /// Opens a programmable device with the specified type and bridge instance
        /// </summary>
        /// <param name="type">The type of programmable device</param>
        /// <param name="bridge">The bridge instance <see cref="IBridge"/></param>
        /// <returns>An instance of <see cref="IDevice"/></returns>
        public static IDevice Open(string type, IBridge bridge)
        {
            switch (type)
            {
                case SupportedDeviceType.AT89S52:
                    return new AT89S52(bridge);
                
                default:
                    break;
            }

            throw new NotFoundException("The device was not found");
        }
    }
}

