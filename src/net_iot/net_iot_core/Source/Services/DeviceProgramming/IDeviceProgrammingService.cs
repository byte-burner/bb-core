using net_iot_core.Services.DeviceProgramming.Models;
using net_iot_core.Shared.ResultHandling;

namespace net_iot_core.Services.DeviceProgramming
{
    /// <summary>
    /// Interface for a service responsible for programming devices.
    /// </summary>
    public interface IDeviceProgrammingService
    {
        List<DeviceInfo> SupportedDevices { get; }

        List<BridgeInfo> SupportedBridges { get; }

        /// <summary>
        /// Retrieves bridge information about all connected bridges.
        /// </summary>
        /// <returns>A service result containing information about all connected bridges.</returns>
        ServiceResult<IEnumerable<BridgeInfo>> GetAllConnectedBridges();

        /// <summary>
        /// Starts monitoring bridge events to retreive connected/disconnected bridge device information from a callback.
        /// </summary>
        /// <param name="onBridgeEvent">The action to perform when a bridge event occurs.</param>
        void StartMonitoringBridgeEvents(Action<IEnumerable<BridgeInfo>> onBridgeEvent);

        /// <summary>
        /// Gets the supported file extensions for a specific device type.
        /// </summary>
        /// <param name="type">The type of the programmable device.</param>
        /// <returns>A service result containing the supported file extensions.</returns>
        ServiceResult<string[]> GetSupportedFileExtensionsByDeviceType(string type);

        /// <summary>
        /// Programs a device using the specified bridge, device type, and program file path.
        /// </summary>
        /// <param name="bridgeType">The type of bridge to use for programming.</param>
        /// <param name="bridgeSerialNbr">The serial number of the bridge.</param>
        /// <param name="deviceType">The type of the programmable device.</param>
        /// <param name="programFilePath">The file path of the program to be loaded onto the device.</param>
        /// <returns>A service result indicating the outcome of the programming operation.</returns>
        ServiceResult ProgramDevice(string bridgeType,
            string bridgeSerialNbr,
            string deviceType,
            string programFilePath);

        /// <summary>
        /// Programs a device using the specified bridge, device type, and program file info.
        /// </summary>
        /// <param name="bridgeType">The type of bridge to use for programming.</param>
        /// <param name="bridgeSerialNbr">The serial number of the bridge.</param>
        /// <param name="deviceType">The type of the programmable device.</param>
        /// <param name="programFileInfo">The program file to be loaded onto the device.</param>
        /// <returns>A service result indicating the outcome of the programming operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the bridge or device type is not found or supported.</exception>
        /// <exception cref="AlreadyInitializedException">Thrown when the bridge or device is already initialized.</exception>
        /// <exception cref="NotInitializedException">Thrown when the bridge or device is not initialized.</exception>
        /// <exception cref="BridgeIOException">Thrown when the bridge or device is already opened.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the program file is not found.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the program file is not found.</exception>
        /// <exception cref="NotImplementedException">Thrown when the extension for the program file is not supported.</exception>
        /// <exception cref="FileFormatException">Thrown when the format of the program file is incorrect.</exception>
        ServiceResult ProgramDevice(string bridgeType,
            string bridgeSerialNbr,
            string deviceType,
            FileInfo programFileInfo);

        /// <summary>
        /// Programs a device using the default bridge, device type, and program file path.
        /// </summary>
        /// <param name="deviceType">The type of the programmable device.</param>
        /// <param name="programFilePath">The file path of the program to be loaded onto the device.</param>
        /// <returns>A service result indicating the outcome of the programming operation.</returns>
        ServiceResult ProgramDevice(string deviceType, string programFilePath);

        /// <summary>
        /// Programs a device using the default bridge, device type, and program file info.
        /// </summary>
        /// <param name="deviceType">The type of the programmable device.</param>
        /// <param name="programFileInfo">The program file to be loaded onto the device.</param>
        /// <returns>A service result indicating the outcome of the programming operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the bridge or device type is not found or supported.</exception>
        /// <exception cref="AlreadyInitializedException">Thrown when the bridge or device is already initialized.</exception>
        /// <exception cref="NotInitializedException">Thrown when the bridge or device is not initialized.</exception>
        /// <exception cref="BridgeIOException">Thrown when the bridge or device is already opened.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the program file is not found.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the program file is not found.</exception>
        /// <exception cref="NotImplementedException">Thrown when the extension for the program file is not supported.</exception>
        /// <exception cref="FileFormatException">Thrown when the format of the program file is incorrect.</exception>
        ServiceResult ProgramDevice(string deviceType, FileInfo programFileInfo);
    }
}
