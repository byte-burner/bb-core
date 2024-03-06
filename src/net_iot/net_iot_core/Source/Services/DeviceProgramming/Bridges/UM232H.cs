using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using Iot.Device.Ft232H;
using net_iot_core.Services.DeviceProgramming.Exceptions;
using net_iot_core.Services.DeviceProgramming.Models;

namespace net_iot_core.Services.DeviceProgramming.Bridges
{
    /// <summary>
    /// Represents a bridge class for UM232H device.
    /// Data Sheet: https://ftdichip.com/wp-content/uploads/2020/07/DS_UM232H.pdf
    /// </summary>
    public class UM232H : IBridge
    {
        #region Private Fields
        private Ft232HDevice? _bridge;
        private GpioController? _gpioController;
        private SpiDevice? _spiDevice;
        private I2cDevice? _i2cDevice;
        private readonly object bridgeLock = new object();
        private static readonly object staticBridgeLock = new object();
        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void ConfigureAsSPIBridge(int busId, int frequ, bool msbFirst = true, int chipSelectLine = -1)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice != null || this._i2cDevice != null)
            {
                throw new AlreadyInitializedException("The SPI or I2C device is already initialized.");
            }

            var settings = new SpiConnectionSettings(busId, chipSelectLine)
            {
                ClockFrequency = frequ,
                DataFlow = msbFirst ? DataFlow.MsbFirst : DataFlow.LsbFirst,
                Mode = SpiMode.Mode0 // 90% of devices use mode0 but may need to allow for external configuration eventually
            };

            try
            {
                _spiDevice = _bridge!.CreateSpiDevice(settings);
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("An I/O error occurred while opening the SPI device. It may be opened by another thread or process.", ex);
            }
        }

        /// <inheritdoc/>
        public void ConfigureAsI2CBridge(int busId, int devId)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice != null || this._i2cDevice != null)
            {
                throw new AlreadyInitializedException("The SPI or I2C device is already initialized.");
            }

            var settings = new I2cConnectionSettings(busId, devId);

            try
            {
                _i2cDevice = _bridge!.CreateI2cDevice(settings);
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("An I/O error occurred while opening the I2C device. It may be opened by another thread or process.", ex);
            }
        }

        /// <inheritdoc/>
        public void WriteToBridge(byte[] dataToWrite)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice == null && this._i2cDevice == null)
            {
                throw new NotInitializedException("The SPI or I2C device is not configured");
            }

            // write to the first device that's not null
            try
            {
                if (_spiDevice != null)
                {
                    _spiDevice.Write(dataToWrite);
                }
                else if (_i2cDevice != null)
                {
                    _i2cDevice.Write(dataToWrite);
                }
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public void WriteToBridge(byte dataToWrite)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice == null && this._i2cDevice == null)
            {
                throw new NotInitializedException("The SPI or I2C device is not configured");
            }

            try
            {
                // write to the first device that's not null
                if (_spiDevice != null)
                {
                    _spiDevice.WriteByte(dataToWrite);
                }
                else if (_i2cDevice != null)
                {
                    _i2cDevice.WriteByte(dataToWrite);
                }
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public byte[] ReadFromBridge(int numBytesToRead)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice == null && this._i2cDevice == null)
            {
                throw new NotInitializedException("The SPI or I2C device is not configured");
            }

            try
            {
                byte[] buff = new byte[numBytesToRead];
                if (_spiDevice != null)
                {
                    _spiDevice.Read(buff);
                }
                else if (_i2cDevice != null)
                {
                    _i2cDevice.Read(buff);
                }

                return buff;
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public byte ReadFromBridge()
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (this._spiDevice == null && this._i2cDevice == null)
            {
                throw new NotInitializedException("The SPI or I2C device is not configured");
            }

            try
            {
                // write to the first device that's not null
                if (_spiDevice != null)
                {
                    return _spiDevice.ReadByte();
                }
                else if (_i2cDevice != null)
                {
                    return _i2cDevice.ReadByte();
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public void WriteToBridgeOnPin(int pinNumber, bool signal = true)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (_gpioController == null)
            {
                _gpioController = _bridge!.CreateGpioController();
            }

            try
            {
                GpioPin pinToWrite = _gpioController.OpenPin(pinNumber, PinMode.Output);

                pinToWrite.Write(signal ? PinValue.High : PinValue.Low);

                _gpioController.ClosePin(pinNumber);
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public bool ReadFromBridgeOnPin(int pinNumber)
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            if (_gpioController == null)
            {
                _gpioController = _bridge!.CreateGpioController();
            }

            try
            {
                GpioPin pinToRead = _gpioController.OpenPin(pinNumber, PinMode.Output);

                var pinVal = pinToRead.Read();

                _gpioController.ClosePin(pinNumber);
                
                return  (bool)pinVal;
            }
            catch (Exception ex)
            {
                throw new BridgeIOException("Cannot read or write. The underlying device may have been disposed, closed or disconnected.", ex);
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            if (this._bridge == null)
            {
                throw new NotInitializedException("The bridge is not initialized.");
            }

            _bridge!.Reset();

            _spiDevice = null;
            _i2cDevice = null;
            _gpioController = null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                if (this._bridge == null)
                {
                    throw new NotInitializedException("The bridge is not initialized.");
                }

                // Calling dispose on the device and the bridge is hacky
                // When PR: https://github.com/dotnet/iot/pull/2270 goes through we no longer
                // need to dispose the SPI device since the root Dispose will dispose all managers then...
                _spiDevice?.Dispose(); 
                _bridge.Dispose();
                _spiDevice = null;
                _i2cDevice = null;
                _gpioController = null;
            }
        }

        /// <inheritdoc/>
        public void Open()
        {
            lock(bridgeLock)
            {
                if (this._bridge != null)
                {
                    throw new AlreadyInitializedException("The bridge is already iniitialized");
                }

                this._bridge = Ft232HDevice.GetFt232H().FirstOrDefault();

                if (this._bridge == null)
                {
                    throw new NotFoundException("The device was not found");
                }

                try
                {
                    // try to use the device to check if the file handle is open already
                    // the QueryComponentInformation call will give us an IO exception if
                    // the file handle for the bridge is already opened
                    this._bridge?.QueryComponentInformation();
                }
                catch (IOException ex)
                {
                    throw new BridgeIOException("An I/O error occurred while opening the device. It may be opened by another thread or process.", ex);
                }
            }
        }

        /// <inheritdoc/>
        public void Open(string serialNbr)
        {
            lock(bridgeLock)
            {
                if (this._bridge != null)
                {
                    throw new AlreadyInitializedException("The bridge is already iniitialized");
                }

                this._bridge = Ft232HDevice.GetFt232H()
                    .Where(b => b.SerialNumber == serialNbr)
                    .FirstOrDefault();

                if (this._bridge == null)
                {
                    throw new NotFoundException("The device was not found");
                }

                try
                {
                    // try to use the device to check if the file handle is open already
                    // the QueryComponentInformation call will give us an IO exception if
                    // the file handle for the bridge is already opened
                    this._bridge?.QueryComponentInformation();
                }
                catch (IOException ex)
                {
                    throw new BridgeIOException("An I/O error occurred while opening the device. It may be opened by another thread or process.", ex);
                }
            }
        }

        /// <summary>
        /// Gets all connected bridges.
        /// </summary>
        /// <returns>A collection of <see cref="BridgeInfo"/> representing the connected bridges.</returns>
        public static IEnumerable<BridgeInfo> GetAllConnectedBridges()
        {
            lock (staticBridgeLock)
            {
                return from b in Ft232HDevice.GetFtx232H()
                    join sb in Bridge.SupportedBridges on b.Id equals sb.DeviceID
                    select new BridgeInfo()
                    {
                        Type = sb.Type,
                        VendorID = sb.VendorID,
                        ProductID = sb.ProductID,
                        DeviceID = sb.DeviceID,
                        SerialNbr = b.SerialNumber,
                    };
            }
        }

        /// <summary>
        /// Checks if a bridge is connected.
        /// </summary>
        /// <returns><c>true</c> if a bridge is connected; otherwise, <c>false</c>.</returns>
        public static bool IsBridgeConnected()
        {
            lock (staticBridgeLock)
            {
                return Ft232HDevice.GetFtx232H().FirstOrDefault() != null;
            }
        }

        #endregion
    }
}
