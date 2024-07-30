using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class LoginSerivce : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "Login";

        public LoginSerivce(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<UsersDto>> LoginAsync(UsersDto dto)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{serviceName}/Login",
                Parameter = dto
            };

            return await client.ExecuteAsync<UsersDto>(request);
        }

        public async Task<ApiResponse> RegisterAsync(UsersDto dto)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{serviceName}/Register",
                Parameter = dto
            };

            return await client.ExecuteAsync(request);
        }
    }
}
