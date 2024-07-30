using LogisticsSystem.Shared.Contact;
using LogisticsSystem.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient client;
        private readonly string serviceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            this.client = client;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{serviceName}/Add",
                Parameter = entity
            };

            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.DELETE,
                Route = $"api/{serviceName}/Delete?id={id}"
            };

            return await client.ExecuteAsync<TEntity>(request);
        }

        public virtual Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.GET,
                Route = $"api/{serviceName}/GetAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" +
                $"&customerId={parameter.CustomerId}"
            };

            return client.ExecuteAsync<PagedList<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.GET,
                Route = $"api/{serviceName}/Get?id={id}"
            };

            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.PUT,
                Route = $"api/{serviceName}/Update",
                Parameter = entity
            };

            return await client.ExecuteAsync<TEntity>(request);
        }
    }
}
