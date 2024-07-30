namespace LogisticsSystem.API.Context
{
    public class AirTransport : BaseEntity
    {
        public int? UserId { get; set; }
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public string SourcePlace { get; set; }
        public string TargetPlace { get; set; }
        public DateTime? SendData { get; set; }
        public string Batch { get; set; }
        public string Note { get; set; }
        public double CalcWeight { get; set; }
        public double Price { get; set; }
        public double OtherMoney { get; set; }
        public string OtherDescription { get; set; }
        public double PayMoney { get; set; }
        public string PayDescription { get; set; }
        public DateTime? PayDate { get; set; }
    }
}
