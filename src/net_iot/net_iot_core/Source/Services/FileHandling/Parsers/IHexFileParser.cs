using net_iot_core.Services.FileHandling.Parsers.Models;

namespace net_iot_core.Services.FileHandling.Parsers
{
    public interface IHexFileParser
    {
        /// <summary>
        /// Parses the Intel Hex file and processes memory entries using the provided action.
        /// </summary>
        /// <param name="processFn">The action to process memory entries.</param>
        /// <param name="maxNumBytes">The maximum number of bytes to process.</param>
        void Parse(Action<IEnumerable<MemoryEntry>> processFn, int maxNumBytes);        

        /// <summary>
        /// Parses the Intel Hex file and processes page entries using the provided action.
        /// </summary>
        /// <param name="processFn">The action to process page entries.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="maxNumPages">The maximum number of pages to process.</param>
        void Parse(Action<PageEntry> processFn, int pageSize, int maxNumPages);        
    }
}