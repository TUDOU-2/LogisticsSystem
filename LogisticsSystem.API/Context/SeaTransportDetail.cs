namespace LogisticsSystem.API.Context
{
    public class SeaTransportDetail : BaseEntity
    {
        public int SeaTransportId { get; set; }
        public int USerId { get; set; }
        public string Productor { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public string Note { get; set; }
    }
}
