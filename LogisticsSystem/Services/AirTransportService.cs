using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class AirTransportService : BaseService<AirTransportDto>, IAirTransportService
    {

        public AirTransportService(HttpRestClient client) : base(client, "AirTransport")
        {
        }
    }
}
