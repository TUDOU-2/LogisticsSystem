using LogisticsSystem.Api;

namespace LogisticsSystem.API.Context.Repository
{
    public class ExpressTransportRepository : Repository<ExpressTransport>, IRepository<ExpressTransport>
    {
        public ExpressTransportRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
