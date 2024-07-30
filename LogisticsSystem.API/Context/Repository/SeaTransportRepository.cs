using LogisticsSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context.Repository
{
    public class SeaTransportRepository : Repository<SeaTransport>, IRepository<SeaTransport>
    {
        public SeaTransportRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
