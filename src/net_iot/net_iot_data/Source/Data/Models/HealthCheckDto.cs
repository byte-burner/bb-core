namespace net_iot_data.Data.Models
{
    public class HealthCheckDto
    {
        public int HealthCheckKey { get; set; }

        public bool Success { get; set; }

        public string? UserId { get; set; }

        public string? Action { get; set; }

        public string? PageId { get; set; }

        public string? LastUpdatedBy { get; set; }

        public string? EstablishedBy { get; set; }

        public DateTime EstablishedDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }
    } 
}

