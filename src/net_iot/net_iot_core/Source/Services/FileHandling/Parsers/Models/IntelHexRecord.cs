namespace net_iot_core.Services.FileHandling.Parsers.Models
{
    public class IntelHexRecord
    {
        public char StartCode { get; set; }
        public byte ByteCount { get; set; }
        public UInt16 Address { get; set; }
        public byte RecordType { get; set; }
        public byte[]? Data { get; set; }
        public byte Checksum { get; set; }
    }
}