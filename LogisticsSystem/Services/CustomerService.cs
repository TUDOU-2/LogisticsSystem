﻿using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class CustomerService : BaseService<CustomerDto>, ICustomerService
    {
        public CustomerService(HttpRestClient client) : base(client, "Customer")
        {

        }
    }
}
