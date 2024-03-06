using Microsoft.Extensions.Logging;
using net_iot_core.Services.DeviceProgramming.Bridges;
using net_iot_core.Services.DeviceProgramming.Devices;
using net_iot_core.Services.DeviceProgramming.Exceptions;
using net_iot_core.Services.DeviceProgramming.Models;
using net_iot_core.Shared.ResultHandling;
using net_iot_core.Shared.ResultHandling.ErrorCodes;

namespace net_iot_core.Services.DeviceProgramming
{
    /// <summary>
    /// Service responsible for programming devices using bridges.
    /// </summary>
    public class DeviceProgrammingService : IDeviceProgrammingService
    {
        private readonly ILogger<DeviceProgrammingService> _logger; 

        public DeviceProgrammingService(ILogger<DeviceProgrammingService> logger)
        {
            this._logger = logger;
        }

        public List<DeviceInfo> SupportedDevices => Device.SupportedDevices;

        public List<BridgeInfo> SupportedBridges => Bridge.SupportedBridges;

        /// <inheritdoc/>
        public ServiceResult<IEnumerable<BridgeInfo>> GetAllConnectedBridges()
        {
            var bridgeTypes = Bridge.GetAllConnectedBridges();

            return new ServiceResult<IEnumerable<BridgeInfo>>() { Payload = bridgeTypes };
        }

        /// <inheritdoc/>
        public void StartMonitoringBridgeEvents(Action<IEnumerable<BridgeInfo>> onBridgeEvent)
        {
            Bridge.StartMonitoringBridgeEvents(onBridgeEvent);
        }

        /// <inheritdoc/>
        public ServiceResult<string[]> GetSupportedFileExtensionsByDeviceType(string type)
        {
            ServiceResult<string[]> result = new ServiceResult<string[]>();

            try
            {
                result.Payload = Device.GetSupportedFileExtensionsByDeviceType(type);
            }
            catch (NotFoundException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.NotFound, type);
                this._logger.LogError(ex, "Could not find device type because it isn't supported");
            }

            return result;
        }

        /// <inheritdoc/>
        public ServiceResult ProgramDevice(
            string bridgeType,
            string bridgeSerialNbr,
            string deviceType,
            string programFilePath)
        {
            return this.ProgramDevice(bridgeType, bridgeSerialNbr, deviceType, new FileInfo(programFilePath));
        }

        /// <inheritdoc/>
        public ServiceResult ProgramDevice(
            string bridgeType,
            string bridgeSerialNbr,
            string deviceType,
            FileInfo programFileInfo)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                // the using statement ensures that dispose is called if an excception is thrown or not
                // so that all resources managed by the bridge are properly disposed
                using (var bridge = Bridge.Open(bridgeType, bridgeSerialNbr))
                {
                    // open the device
                    var device = Device.Open(deviceType, bridge);

                    // program the device
                    device.Program(programFileInfo!);
                }
            }
            catch (NotFoundException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.NotFound, deviceType);
                this._logger.LogError(ex, "Could not find bridge or device type because it isn't supported");
            }
            catch (AlreadyInitializedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.AlreadyInitialized, deviceType);
                this._logger.LogError(ex, "The bridge or device is already initialized");
            }
            catch (NotInitializedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.NotInitialized, deviceType);
                this._logger.LogError(ex, "The bridge or device is not initialized");
            }
            catch (BridgeIOException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.AlreadyOpen, deviceType);
                this._logger.LogError(ex, "The bridge or device is already opened");
            }
            catch (IOException ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileNotFound, programFileInfo.FullName);
                this._logger.LogError(ex, "The file was not found");
            }
            catch (NotImplementedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileExtensionNotSupported, programFileInfo.FullName);
                this._logger.LogError(ex, "Extension for the program file is not supported");
            }
            catch (FileFormatException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileFormatIsIncorrect, programFileInfo.FullName);
                this._logger.LogError(ex, "Invalid file format");
            }

            return result;
        }

        /// <inheritdoc/>
        public ServiceResult ProgramDevice(
            string deviceType,
            string programFilePath)
        {
            return this.ProgramDevice(deviceType, new FileInfo(programFilePath));
        }

        /// <inheritdoc/>
        public ServiceResult ProgramDevice(
            string deviceType,
            FileInfo programFileInfo)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                // the using statement ensures that dispose is called if an excception is thrown or not
                // so that all resources managed by the bridge are properly disposed
                using (var bridge = Bridge.Open())
                {
                    // open the device
                    var device = Device.Open(deviceType, bridge);

                    // program the device
                    device.Program(programFileInfo!);
                }
            }
            catch (NotFoundException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.NotFound, deviceType);
                this._logger.LogError(ex, "Could not find bridge or device type because it isn't supported");
            }
            catch (AlreadyInitializedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.AlreadyInitialized, deviceType);
                this._logger.LogError(ex, "The bridge or device is already initialized");
            }
            catch (NotInitializedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.NotInitialized, deviceType);
                this._logger.LogError(ex, "The bridge or device is not initialized");
            }
            catch (BridgeIOException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.AlreadyOpen, deviceType);
                this._logger.LogError(ex, "The bridge or device is already opened");
            }
            catch (IOException ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileNotFound, programFileInfo.FullName);
                this._logger.LogError(ex, "The file was not found");
            }
            catch (NotImplementedException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileExtensionNotSupported, programFileInfo.FullName);
                this._logger.LogError(ex, "Extension for the program file is not supported");
            }
            catch (FileFormatException ex)
            {
                result.Add(DeviceProgrammingErrorCodes.ProgramFileFormatIsIncorrect, programFileInfo.FullName);
                this._logger.LogError(ex, "Invalid file format");
            }

            return result;
        }
    }
}