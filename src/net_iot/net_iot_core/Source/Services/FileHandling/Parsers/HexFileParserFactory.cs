using Microsoft.Extensions.Logging;

namespace net_iot_core.Services.FileHandling.Parsers
{
    /// <summary>
    /// Factory class responsible for creating instances of hex file parsers.
    /// </summary>
    public class HexFileParserFactory : IHexFileParserFactory
    {
        private readonly ILogger<HexFileParserFactory> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HexFileParserFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public HexFileParserFactory(ILogger<HexFileParserFactory> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Creates an instance of a hex file parser based on the file extension.
        /// </summary>
        /// <param name="file">The file for which to create the parser.</param>
        /// <returns>An instance of <see cref="IHexFileParser"/>.</returns>
        public static IHexFileParser Create(FileInfo file)
        {
            switch (file.Extension)
            {
                case ".ihx":
                    return new IntelHexFileParser(file);

                // add more cases here
                
                default:
                    break;
            }

            throw new NotImplementedException($"The hex parser does not support the file type extension of {file.Extension}");
        }
    }
}