using net_iot_core.Services.DeviceProgramming.Bridges;
using net_iot_core.Services.DeviceProgramming.Constants;
using net_iot_core.Services.FileHandling;
using net_iot_core.Services.FileHandling.Parsers;
using net_iot_core.Services.FileHandling.Parsers.Models;

namespace net_iot_core.Services.DeviceProgramming.Devices
{
    /// <summary>
    /// Represents the programming instruction set for the AT89S52 microcontroller device for in-system programming.
    /// Data Sheet: https://ww1.microchip.com/downloads/en/DeviceDoc/doc1919.pdf
    /// </summary>
    public static class ProgrammingInstructionSet 
    {
        
        public static readonly byte[] ProgrammingEnable = new byte[4]{ 0b10101100, 0b01010011, 0b00000000, 0b00000000 };

        public static readonly byte[] ChipErase = new byte[4]{ 0b10101100, 0b10000000, 0b00000000, 0b00000000 };

        public static readonly byte ReadProgramMemoryInByteMode = 0b00100000;

        public static readonly byte WriteProgramMemoryInByteMode = 0b01000000;

        public static readonly byte ReadProgramMemoryInPageMode = 0b00110000;

        public static readonly byte WriteProgramMemoryInPageMode = 0b01010000;
    }


    /// <summary>
    /// Represents the AT89S52 microcontroller device for in-system programming.
    /// Data Sheet: https://ww1.microchip.com/downloads/en/DeviceDoc/doc1919.pdf
    /// </summary>
    public class AT89S52 : IDevice
    {
        #region Private Variables
        private readonly IBridge _bridge;

        private ProgrammingMode _mode = ProgrammingMode.PAGE; // AT89S52 will be programmed using page mode by default

        private int _pageSize = 256; // AT89S52 has a page size of 256 bytes

        private int _maxNumPages = 8; // AT89S52 can have up to 8 pages

        private int _maxNumBytes = 8192; // AT89S52 has 8K Bytes of In-System Programmable (ISP) Flash Memory

        private int _resetPin = 4; // Arbitrary pin value for the bridge device to use

        private int _maxFrequ = 500000; // Measured in Hz ==> must be less than 1/16 the system clock at XTAL1 (assuming 12Mhz)
        
        private bool _isMsbFirst = true; // AT89S52 is programmed via MSB first

        private int _chipSelectLine = 3; // must be in between 3 and 15

        public static string[] SupportedFileExtensions => new[]
        {
            ".ihx",
        };

        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AT89S52"/> class with the specified bridge.
        /// </summary>
        /// <param name="bridge">The bridge interface for communication with the device.</param>
        public AT89S52(IBridge bridge)
        {
            if (bridge == null)
            {
               throw new ArgumentNullException("bridge is null"); 
            }

            this._bridge = bridge;

            // The AT89S52 communicates via SPI to its internal ISP programmer so set the bridge as a SPI device
            this._bridge.ConfigureAsSPIBridge(
                busId: 1,
                frequ: this._maxFrequ, 
                msbFirst: this._isMsbFirst, 
                chipSelectLine: this._chipSelectLine); 
        }

        #endregion

        #region Public Methods
        /// <inheritdoc/>
        public void Wipe()
        {
            // 1. call the enable method
            this.Enable();

            // 2. call the chip erase method
            this.Erase();

            // 3. call reset method
            this.Reset();
        }

        /// <inheritdoc/>
        public void SetProgrammingMode(ProgrammingMode mode)
        {
            this._mode = mode;
        }

        /// <inheritdoc/>
        public void Program(string filePath)
        {
            this.Program(new FileInfo(filePath));
        }

        /// <inheritdoc/>
        public void Program(FileInfo fileInfo)
        {
            // 1. call the enable method
            this.Enable();

            // 2. call the chip erase method
            this.Erase();

            // 3. parse the file and process the memory entries on the fly by passing in the write method as a callback to the parse method
                // - A memory entry is an address byte array and a data byte stored together. MemoryEntry is an abstraction of the file parser.
                // The file parser takes the ihx file and creates memory entry on each iteration that is then processed immediately. This is memory
                // efficient so that all bytes in file arent stored in memory and processed at once, this way we can chunk the data into a buffer
                // and process it in batches. And that all happens under the hood in the FileStream class that is utilized by the StreamReader in
                // the parser...
                // - a max of 8 pages can be stored in AT89S52
            IHexFileParser parser = FileHandlingService.Create(fileInfo);

            if (this._mode == ProgrammingMode.BYTE)
            {
                parser.Parse(WriteProgramInByteMode, this._maxNumBytes);
            }
            else if (this._mode == ProgrammingMode.PAGE)
            {
                parser.Parse(WriteProgramInPageMode, this._pageSize, this._maxNumPages);
            }

            // 4. call the reset method to begin program execution
            this.Reset();
        }

        #endregion

        #region Private Methods

        private void WriteProgramInByteMode(IEnumerable<MemoryEntry> memoryEntries)
        {
            foreach (var memoryEntry in memoryEntries)
            {
                this.WriteByByteMode(BitConverter.GetBytes(memoryEntry.Address!), memoryEntry.Data);
            }
        }

        private void WriteProgramInPageMode(PageEntry pageEntry)
        {
            this.WriteByPageMode(pageEntry.PageNumber, pageEntry.Page!);
        }

        #region Programming Instruction Set
        /*
         * The following methods make up the programming instruction set for the AT89S52.
         * On page 27 table 24-1 at https://ww1.microchip.com/downloads/en/DeviceDoc/doc1919.pdf
         */

        private void Enable()
        {
            // 1. set the reset pin to high
            _bridge.WriteToBridgeOnPin(pinNumber: this._resetPin, signal: true);

            // 2. Wait for programming to initialize
                // - After Reset signal is high, SCK should be low for at least 64 system clocks before it goes high
                // to clock in the enable data bytes. No pulsing of Reset signal is necessary. SCK should be no faster
                // than 1/16 of the system clock at XTAL1.
                // - need to wait 64 system clock cycles before executing any next instruction
                // - 3 Mhz is slowest clock can go for AT89S52 ==> 3Mhz = 0.00033333333333333ms ==> 64 * 0.00033333333333333ms = 0.02133333333
                // - Therefore, the thread has to sleep for a period of time that's >= 0.02133333333
            Thread.Sleep(1);
            
            // 2. execute the chip enable instruction
            _bridge.WriteToBridge(ProgrammingInstructionSet.ProgrammingEnable);

            // allow time for the chip to become enabled
            Thread.Sleep(200);
        }

        private void Erase()
        {
            // 1. execute the chip erase instruction
            _bridge.WriteToBridge(ProgrammingInstructionSet.ChipErase);

            // allow time for the flash memory to flush itself
            Thread.Sleep(200);
        }

        /// <summary>
        /// Read a single byte from a memory address
        /// </summary>
        /// <param name="address">The memory address to read data from.</param>
        private byte ReadByByteMode(byte[] address)
        {
            _bridge.WriteToBridge(new byte[]
            {
                ProgrammingInstructionSet.ReadProgramMemoryInByteMode,
                address[1],
                address[0],
                0b00000000,
            });

            return _bridge.ReadFromBridge();
        }

        /// <summary>
        /// Write a single byte to a memory address
        /// </summary>
        /// <param name="address">The memory address to write data to.</param>
        /// <param name="data">The bitewise data to send to memory.</param>
        private void WriteByByteMode(byte[] address, byte data)
        {
            _bridge.WriteToBridge(new byte[]
            {
                ProgrammingInstructionSet.WriteProgramMemoryInByteMode,
                address[1],
                address[0],
                data
            });

            // AT89S52 write cycle takes less thaan 0.5ms so we need to sleep in between each write atleast 0.5ms
            Thread.Sleep(1);
        }

        private void WriteLockBits()
        {
            throw new NotImplementedException();
        }

        private void ReadLockBits()
        {
            throw new NotImplementedException();
        }
        private void ReadSignatureBytes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads data from the AT89S52 device memory in page mode.
        /// </summary>
        /// <param name="pageNbr">The page number from which to read data.</param>
        /// <returns>The data read from the device memory.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown if the page number argument is outside the valid page range.
        /// </exception>
        private byte[] ReadByPageMode(byte pageNbr)
        {
            // max/min nbr of pages in AT89S52
            if (pageNbr > this._pageSize || pageNbr < 0)
            {
                throw new IndexOutOfRangeException("Page number argument must be in the correct page range");
            }

            // write page to AT89S52 through SPI bridge
            _bridge.WriteToBridge(new byte[]
            {
                ProgrammingInstructionSet.ReadProgramMemoryInPageMode,
                pageNbr
            });

            byte[] buff = _bridge.ReadFromBridge(this._pageSize); 

            return buff;
        }

        /// <summary>
        /// Writes data to the AT89S52 device memory in page mode.
        /// </summary>
        /// <param name="pageNbr">The page number to write data to.</param>
        /// <param name="page">The data to write to the device memory.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown if the page number argument is outside the valid page range,
        /// or if the length of the data array exceeds the maximum page size.
        /// </exception>
        private void WriteByPageMode(byte pageNbr, byte[] page)
        {
            // max/min nbr of pages in AT89S52
            if (pageNbr > this._pageSize || pageNbr < 0)
            {
                throw new IndexOutOfRangeException("Page number argument must be in the correct page range");
            }

            // max/min nbr of pages in AT89S52
            if (page.Length > this._pageSize)
            {
                throw new IndexOutOfRangeException("Length of data array must be in the correct page range");
            }

            // create page mode command
            _bridge.WriteToBridge(new byte[]
            {
                ProgrammingInstructionSet.WriteProgramMemoryInPageMode,
                pageNbr
            });

            // AT89S52 write cycle takes less thaan 0.5ms so we need to sleep in between each write atleast 0.5ms
            Thread.Sleep(1);

            // write page to AT89S52 through SPI bridge one byte at a time per AT89S52 requirement on datasheet:
            // "The Code array is programmed one byte at a time in either the Byte or Page mode. The
            // write cycle is self-timed and typically takes less than 0.5 ms at 5V."
            foreach (var dataByte in page)
            {
                _bridge.WriteToBridge(dataByte);

                // AT89S52 write cycle takes less thaan 0.5ms so we need to sleep in between each write atleast 0.5ms
                Thread.Sleep(1);
            }
        }

        private void Reset()
        {
            // 1. set the reset pin to low to commence normal program operation
            _bridge.WriteToBridgeOnPin(pinNumber: this._resetPin, signal: false);

            // Allow the AT89S52 reset operation to complete
            Thread.Sleep(500);
        }

        #endregion

        #endregion
    }
}