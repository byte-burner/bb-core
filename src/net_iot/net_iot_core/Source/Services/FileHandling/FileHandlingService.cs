using net_iot_core.Services.FileHandling.Parsers;

namespace net_iot_core.Services.FileHandling
{
    /// <summary>
    /// Service for handling file operations for parsing hex files.
    /// </summary>
    public class FileHandlingService : IFileHandlingService
    {
        /// <summary>
        /// Creates a hex file parser based on the provided file information.
        /// </summary>
        /// <param name="fileInfo">Information about the file to parse.</param>
        /// <returns>An instance of a hex file parser.</returns>
        public static IHexFileParser Create(FileInfo fileInfo)
        {
            return HexFileParserFactory.Create(fileInfo);
        }
    }
}