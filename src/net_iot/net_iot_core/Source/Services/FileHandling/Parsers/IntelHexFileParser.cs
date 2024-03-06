
using net_iot_core.Services.FileHandling.Parsers.Models;

namespace net_iot_core.Services.FileHandling.Parsers
{
    /// <summary>
    /// Parses Intel Hex (.ihx) files to extract memory and page entries.
    /// </summary>
    public class IntelHexFileParser : IHexFileParser
    {
        private readonly FileInfo _file;
        
        private readonly byte EndOfFileRecordType = 0x01;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntelHexFileParser"/> class.
        /// </summary>
        /// <param name="file">The Intel Hex file to parse.</param>
        public IntelHexFileParser(FileInfo file)
        {
            _file = file;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntelHexFileParser"/> class with the file path.
        /// </summary>
        /// <param name="filePath">The file path of the Intel Hex file to parse.</param>
        public IntelHexFileParser(string filePath)
        {
            _file = new FileInfo(filePath);
        }

        /// <inheritdoc/>
        public void Parse(Action<IEnumerable<MemoryEntry>> processFn, int maxNumBytes)
        {
            using (StreamReader sr = _file.OpenText())
            {
                string line;
                bool EOFRecordExists = false;
                long byteCount = 0;
                while((line = sr.ReadLine()!) != null)
                {
                    IntelHexRecord? record;

                    try
                    {
                        record = new IntelHexRecord()
                        {
                            StartCode = line[0],
                            ByteCount = Convert.ToByte(line.Substring(1, 2), 16),
                            Address = Convert.ToUInt16(line.Substring(3, 4), 16),
                            RecordType = Convert.ToByte(line.Substring(7, 2), 16),
                            Data = ConvertHexStringToByteArray(line.Substring(9, line.Length - 11)),
                            Checksum = Convert.ToByte(line.Substring(line.Length - 2, 2), 16),
                        };
                    }
                    catch
                    {
                        throw new FileFormatException("ihx record is malformed. Could not parse file");
                    }

                    ValidateRecord(record);

                    if (record.RecordType == this.EndOfFileRecordType)
                    {
                        EOFRecordExists = true;
                        break; 
                    }
                    else
                    {
                        IEnumerable<MemoryEntry> memoryEntry = ConvertRecordToMemoryEntryList(record);

                        // count the number of bytes
                        byteCount += memoryEntry.Count();

                        if (byteCount > maxNumBytes)
                        {
                            throw new FileFormatException("ihx file holds more bytes than the max number of allowable bytes for programming");
                        }

                        // process memory entry list here
                        processFn(memoryEntry);
                    }
                }

                if (!EOFRecordExists)
                {
                    throw new FileFormatException("No EOF record exists for the ihx file. Program may be malformed");
                }
            }
        }

        /// <inheritdoc/>
        public void Parse(Action<PageEntry> processFn, int pageSize, int maxNumPages)
        {
            using (StreamReader sr = _file.OpenText())
            {
                string line;
                bool EOFRecordExists = false;
                List<MemoryEntry> allMemoryEntries = new List<MemoryEntry>();
                while((line = sr.ReadLine()!) != null)
                {
                    IntelHexRecord? record;

                    try
                    {
                        record = new IntelHexRecord()
                        {
                            StartCode = line[0],
                            ByteCount = Convert.ToByte(line.Substring(1, 2), 16),
                            Address = Convert.ToUInt16(line.Substring(3, 4), 16),
                            RecordType = Convert.ToByte(line.Substring(7, 2), 16),
                            Data = ConvertHexStringToByteArray(line.Substring(9, line.Length - 11)),
                            Checksum = Convert.ToByte(line.Substring(line.Length - 2, 2), 16),
                        };
                    }
                    catch
                    {
                        throw new FileFormatException("ihx record is malformed. Could not parse file");
                    }

                    ValidateRecord(record);

                    if (record.RecordType == this.EndOfFileRecordType)
                    {
                        EOFRecordExists = true;
                        break; 
                    }
                    else
                    {
                        IEnumerable<MemoryEntry> memoryEntries = ConvertRecordToMemoryEntryList(record);

                        // process memory entry list here
                        allMemoryEntries.AddRange(memoryEntries);

                        if (allMemoryEntries.Count > maxNumPages * pageSize)
                        {
                            throw new FileFormatException("ihx file holds more pages than the max number of allowable pages");
                        }
                    }
                }

                if (!EOFRecordExists)
                {
                    throw new FileFormatException("No EOF record exists for the ihx file. Program may be malformed");
                }

                // sort memory entries
                var orderedMemoryEntries = allMemoryEntries.OrderBy(me => me.Address).ToList();

                // convert the memory entries to a list of page entries
                for(int i = 0; i < Math.Ceiling((decimal)orderedMemoryEntries.Count / pageSize); i++)
                {
                    byte[] dest = new byte[pageSize]; 
                    byte[] source = orderedMemoryEntries.Skip(i * pageSize).Take(pageSize).Select(me => me.Data).ToArray();
                    Array.Copy(source, dest, source.Length);

                    var pageEntry = new PageEntry()
                    {
                        PageNumber = Convert.ToByte(i),
                        Page = dest,
                    };

                    processFn(pageEntry);
                }
            }
        }

        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("The hex string length must be an even number.");
            }

            byte[] byteArr = new byte[hexString.Length / 2];
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteArr;
        }

        /// <summary>
        /// Converts an IntelHexRecord to a list of MemoryEntry objects.
        /// </summary>
        /// <param name="record">The IntelHexRecord to convert.</param>
        /// <returns>A list of MemoryEntry objects representing the data in the IntelHexRecord.</returns>
        private IEnumerable<MemoryEntry> ConvertRecordToMemoryEntryList(IntelHexRecord record)
        {
            List<MemoryEntry> memoryEntries = new List<MemoryEntry>();
            UInt16 address = record.Address;
            foreach(var data in record.Data!)
            {
                memoryEntries.Add(new MemoryEntry(){ Address = address, Data = data });
                address++;
            }

            return memoryEntries;
        }

        #region Validation Methods
        private void ValidateRecord(IntelHexRecord record)
        {
            if (record.StartCode != ':')
            {
                throw new FileFormatException("ihx record is malformed. Could not parse file");
            }

            if (!IsValidRecordType(record))
            {
                throw new NotImplementedException("Can only parse Data (0x00) and EOF (0x01) reocrd types (i.e. 0x00)");
            }

            if(!IsValidChecksum(record)){
                throw new FileFormatException("Invalid checksum in ihx file");
            }
        }

        /// <summary>
        /// Checks if the checksum of the IntelHexRecord is valid.
        /// </summary>
        /// <param name="record">The IntelHexRecord to validate.</param>
        /// <returns>True if the checksum is valid, otherwise false.</returns>
        /// <remarks>
        /// The checksum of an IntelHexRecord is calculated as follows:
        /// 1. Sum all data bytes in the record.
        /// 2. Add the values of the ByteCount, RecordType, and Address fields to the sum.
        /// 3. Take the two's complement of the least significant byte (LSB) in the sum.
        /// 4. Compare the calculated checksum with the checksum provided in the IntelHexRecord.
        /// 5. If the calculated checksum matches the provided checksum, the record is considered valid.
        /// </remarks>
        private bool IsValidChecksum(IntelHexRecord record)
        {
            int sum = 0;
            foreach (byte b in record.Data!)
            {
                sum += b;
            }

            sum += (int)record.ByteCount + (int)record.RecordType + (int)record.Address;

            // get two's complement of the LSB in the sum
            byte checksum = (byte)(0x100 - (sum & 0xFF));

            return record.Checksum == checksum;
        }

        /// <summary>
        /// Checks if the record type of the IntelHexRecord is valid.
        /// </summary>
        /// <param name="record">The IntelHexRecord to validate.</param>
        /// <returns>True if the record type is valid, otherwise false.</returns>
        /// <remarks>
        /// The record type of an IntelHexRecord indicates the type of data it represents.
        /// Valid record types include Data (0x00) and End of File (EOF) (0x01).
        /// This method checks if the provided record has a valid record type.
        /// </remarks>
        private bool IsValidRecordType(IntelHexRecord record)
        {
            switch (record.RecordType)
            {
                case 0x00:
                case 0x01:
                    return true;
                
                default:
                    break;
            }

            return false;
        }
        #endregion


    }
}