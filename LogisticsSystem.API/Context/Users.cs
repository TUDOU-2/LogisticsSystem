namespace LogisticsSystem.API.Context
{
    public class Users : BaseEntity
    {
        public string Account { get; set; } 
        public string Name { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
    }
}
