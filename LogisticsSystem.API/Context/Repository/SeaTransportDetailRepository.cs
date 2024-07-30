using LogisticsSystem.Api;

namespace LogisticsSystem.API.Context.Repository
{
    public class SeaTransportDetailRepository : Repository<SeaTransportDetail>, IRepository<SeaTransportDetail>
    {
        public SeaTransportDetailRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
