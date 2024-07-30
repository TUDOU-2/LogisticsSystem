using LogisticsSystem.Shared.Contact;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class SeaTransportService : BaseService<SeaTransportDto>, ISeaTransportService
    {
        public SeaTransportService(HttpRestClient client) : base(client, "SeaTransport")
        {
        }
    }
}
