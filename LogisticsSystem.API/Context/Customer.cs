namespace LogisticsSystem.API.Context
{
    public class Customer : BaseEntity
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Nation { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
