using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Messages
{
    public class EditTransportMessage<TEntityDto>
    {
        public TEntityDto entityDto { get; }
        public List<CustomerDto> CustomerList { get; }

        public EditTransportMessage(TEntityDto value, List<CustomerDto> customerList)
        {
            entityDto = value;
            CustomerList = customerList;
        }
    }
}
