using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class SeaTransportDetailService : BaseService<SeaTransportDetailDto>, ISeaTransportDetailService
    {
        public SeaTransportDetailService(HttpRestClient client) : base(client, "SeaTransportDetail")
        {
        }
    }
}
