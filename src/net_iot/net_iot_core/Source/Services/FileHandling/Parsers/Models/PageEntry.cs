namespace net_iot_core.Services.FileHandling.Parsers.Models
{
    public class PageEntry
    {
        public byte PageNumber { get; set; }
        public byte[]? Page { get; set; }
    }
}
