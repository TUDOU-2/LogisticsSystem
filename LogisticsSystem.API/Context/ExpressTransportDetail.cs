namespace LogisticsSystem.API.Context
{
    public class ExpressTransportDetail : BaseEntity
    {
        public int UserId { get; set; }
        public int ExpressTransportId { get; set; }
        public string Productor { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
    }
}
