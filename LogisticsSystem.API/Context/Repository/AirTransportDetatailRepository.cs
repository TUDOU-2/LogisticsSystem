using LogisticsSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context.Repository
{
    public class AirTransportDetailRepository : Repository<AirTransportDetail>, IRepository<AirTransportDetail>
    {
        public AirTransportDetailRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
