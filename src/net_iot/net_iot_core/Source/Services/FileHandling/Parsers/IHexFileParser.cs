using net_iot_core.Services.FileHandling.Parsers.Models;

namespace net_iot_core.Services.FileHandling.Parsers
{
    public interface IHexFileParser
    {
        /// <summary>
        /// Parses an Intel Hex (ihx) file, processes its records, and invokes a callback function for each memory entry.
        /// </summary>
        /// <param name="processFn">The callback function to invoke for each memory entry parsed from the ihx file.</param>
        /// <param name="maxNumBytes">The maximum number of bytes allowable for programming.</param>
        /// <remarks>
        /// This method reads each line of the Intel Hex (ihx) file, parses it into an IntelHexRecord object,
        /// validates the record, and processes it. If the record represents an End of File (EOF) record,
        /// the parsing process stops. The method ensures that the total number of bytes parsed does not
        /// exceed the maximum allowable number of bytes for programming. It throws various exceptions
        /// if the ihx file or its records are malformed or if the number of bytes exceeds the maximum limit.
        /// </remarks>
        /// <exception cref="FileFormatException">Thrown when the ihx record is malformed or the file cannot be parsed.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the ihx file is not found.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the ihx file is not found.</exception>
        void Parse(Action<IEnumerable<MemoryEntry>> processFn, int maxNumBytes);        

        /// <summary>
        /// Parses an Intel Hex (ihx) file, processes its records, and invokes a callback function for each page entry.
        /// </summary>
        /// <param name="processFn">The callback function to invoke for each page entry parsed from the ihx file.</param>
        /// <param name="pageSize">The size of each page in bytes.</param>
        /// <param name="maxNumPages">The maximum number of pages allowable.</param>
        /// <remarks>
        /// This method reads each line of the Intel Hex (ihx) file, parses it into an IntelHexRecord object,
        /// validates the record, and processes it. If the record represents an End of File (EOF) record,
        /// the parsing process stops. The method ensures that the total number of pages parsed does not
        /// exceed the maximum allowable number of pages. It throws various exceptions if the ihx file
        /// or its records are malformed or if the number of pages exceeds the maximum limit.
        /// Once all memory entries are processed, they are sorted by address, and then divided into pages
        /// according to the specified page size. Each page is converted to a PageEntry object and passed
        /// to the callback function for further processing.
        /// </remarks>
        /// <exception cref="FileFormatException">Thrown when the ihx record is malformed or the file cannot be parsed.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the ihx file is not found.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory of the ihx file is not found.</exception>
        void Parse(Action<PageEntry> processFn, int pageSize, int maxNumPages);        
    }
}