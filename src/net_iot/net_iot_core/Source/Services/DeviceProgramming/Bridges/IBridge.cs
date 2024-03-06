namespace net_iot_core.Services.DeviceProgramming.Bridges
{
    public interface IBridge : IDisposable
    {
        /// <summary>
        /// Opens the bridge.
        /// </summary>
        void Open();

        /// <summary>
        /// Opens the bridge with the specified serial number.
        /// </summary>
        /// <param name="serialNbr">The serial number of the bridge.</param>
        void Open(string serialNbr);

        /// <summary>
        /// Configures the bridge as a SPI device.
        /// </summary>
        /// <param name="busId">The bus ID.</param>
        /// <param name="frequ">The clock frequency.</param>
        /// <param name="msbFirst">Whether to send data in most significant bit first order.</param>
        /// <param name="chipSelectLine">The chip select line number.</param>
        void ConfigureAsSPIBridge(int busId, int frequ, bool msbFirst = true, int chipSelectLine = -1);

        /// <summary>
        /// Configures the bridge as an I2C device.
        /// </summary>
        /// <param name="busId">The bus ID.</param>
        /// <param name="devId">The device ID.</param>
        void ConfigureAsI2CBridge(int busId, int devId);

        /// <summary>
        /// Writes data to the bridge.
        /// </summary>
        /// <param name="dataToWrite">The data to write.</param>
        void WriteToBridge(byte dataToWrite);

        /// <summary>
        /// Writes a byte to the bridge.
        /// </summary>
        /// <param name="dataToWrite">The byte to write.</param>
        void WriteToBridge(byte[] dataToWrite);

        /// <summary>
        /// Reads data from the bridge.
        /// </summary>
        /// <param name="numBytesToRead">The number of bytes to read.</param>
        /// <returns>The read bytes.</returns>
        byte[] ReadFromBridge(int numBytesToRead);

        /// <summary>
        /// Reads a byte from the bridge.
        /// </summary>
        /// <returns>The read byte.</returns>
        byte ReadFromBridge();

        /// <summary>
        /// Writes to a specific pin on the bridge.
        /// </summary>
        /// <param name="pinNumber">The pin number to read from.</param>
        /// <returns>The signal represented as a boolean value.</returns>
        void WriteToBridgeOnPin(int pinNumber, bool signal = true);

        /// <summary>
        /// Reads from a specific pin on the bridge.
        /// </summary>
        /// <param name="pinNumber">The pin number to read from.</param>
        /// <returns>The signal represented as a boolean value.</returns>
        bool ReadFromBridgeOnPin(int pinNumber);

        /// <summary>
        /// Resets the bridge back to its default configuration.
        /// </summary>
        void Reset();
    }
}
