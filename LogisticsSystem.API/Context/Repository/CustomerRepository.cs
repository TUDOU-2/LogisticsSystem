using LogisticsSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.API.Context.Repository
{
    public class CustomerRepository : Repository<Customer>, IRepository<Customer>
    {
        public CustomerRepository(LogisticsSystemContext dbContext) : base(dbContext)
        {
        }
    }
}
