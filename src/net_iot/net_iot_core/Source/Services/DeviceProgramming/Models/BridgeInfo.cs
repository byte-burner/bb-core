namespace net_iot_core.Services.DeviceProgramming.Models
{
    public class BridgeInfo
    {
        public string? Type { get; set; } // universal human readable ID

        public string? VendorID { get; set; } // VID

        public string? ProductID { get; set; } // PID

        public long DeviceID { get; set; } // Device ID - Combination of VID and PID

        public string? SerialNbr { get; set; }

        public string? DevicePath { get; set; }
    }
}
