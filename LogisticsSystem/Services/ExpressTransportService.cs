using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class ExpressTransportService : BaseService<ExpressTransportDto>, IExpressTransportService
    {
        public ExpressTransportService(HttpRestClient client) : base(client, "ExpressTransport")
        {
        }
    }
}
