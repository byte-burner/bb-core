namespace net_iot_data.Data.Entities
{
    public class MessageLogEntity
    {
        public int Id { get; set; }

        public string? AddedDate { get; set; }

        public string? Level { get; set; }

        public string? Message { get; set; }

        public string? Exception { get; set; }
    }
}