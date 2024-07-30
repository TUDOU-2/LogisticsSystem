using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context
{
    public class LogisticsSystemContext : DbContext
    {
        public LogisticsSystemContext(DbContextOptions<LogisticsSystemContext> options) : base(options)
        {
              
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<AirTransport> AirTransport { get; set; }
        public DbSet<AirTransportDetail> AirTransportDetail { get; set; }
        public DbSet<SeaTransport> SeaTransport { get; set; }
        public DbSet<SeaTransportDetail> SeaTransportDetail { get; set; }
        public DbSet<ExpressTransport> ExpressTransport { get; set; }
        public DbSet<ExpressTransportDetail> ExpressTransportDetail { get; set; }
    }
}
