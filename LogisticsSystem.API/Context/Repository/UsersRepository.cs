using LogisticsSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context.Repository
{
    public class UsersRepository : Repository<Users>, IRepository<Users>
    {
        public UsersRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
