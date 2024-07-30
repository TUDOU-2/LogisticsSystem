using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class AirTransportDetailService : BaseService<AirTransportDetailDto>, IAirTransportDetailService
    {
        public AirTransportDetailService(HttpRestClient client) : base(client, "AirTransportDetail")
        {
        }
    }
}
