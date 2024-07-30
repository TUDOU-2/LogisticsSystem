using LogisticsSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context.Repository
{
    public class AirTransportRepository : Repository<AirTransport>, IRepository<AirTransport>
    {
        public AirTransportRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
