using net_iot_core.Services.DeviceProgramming.Constants;

namespace net_iot_core.Services.DeviceProgramming.Devices
{
    public interface IDevice
    {
        /// <summary>
        /// Wipes the device by enabling, erasing, and resetting it.
        /// </summary>
        void Wipe();

        /// <summary>
        /// Sets the programming mode of the device (i.e. how the device will read, write, and communicate data).
        /// </summary>
        /// <param name="mode">The programming mode to set.</param>
        void SetProgrammingMode(ProgrammingMode mode);

        /// <summary>
        /// Programs the device with the firmware from the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the firmware file.</param>
        void Program(string filePath);

        /// <summary>
        /// Programs the device with the firmware from the specified file info.
        /// </summary>
        /// <param name="fileInfo">The firmware file information.</param>
        void Program(FileInfo fileInfo);
    }
}