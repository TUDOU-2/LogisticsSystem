using LogisticsSystem.Api;

namespace LogisticsSystem.API.Context.Repository
{
    public class ExpressTransportDetailRepository : Repository<ExpressTransportDetail>, IRepository<ExpressTransportDetail>
    {
        public ExpressTransportDetailRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
