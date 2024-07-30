using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class ExpressTransportDetailService : BaseService<ExpressTransportDetailDto>, IExpressTransportDetailService
    {
        public ExpressTransportDetailService(HttpRestClient client) : base(client, "ExpressTransportDetail")
        {
        }
    }
}
