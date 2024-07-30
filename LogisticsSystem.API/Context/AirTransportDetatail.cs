namespace LogisticsSystem.API.Context
{
    public class AirTransportDetail : BaseEntity
    {
        public int AirTransportId { get; set; }
        public int? UserId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public string? Note { get; set; }
    }
}
