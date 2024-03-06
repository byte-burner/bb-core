namespace net_iot_util.CommandHandlers.Models
{
    public class FlashCommandOptions : IOptions
    {
        public string? DeviceTypeOption { get; set; }

        public FileInfo? HexFileOption { get; set; }

        public string? BridgeTypeOption { get; set; }

        public string? BridgeSerialNumberOption { get; set; }

        public bool EraseOption { get; set; }

        public FileInfo? ConfFileOption { get; set; }
    }
}