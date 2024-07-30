using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Services
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }
            else
            {
                return new ApiResponse
                {
                    Status = false,
                    Message = $"Error: {response.StatusCode} - {response.ErrorMessage}"
                };
            }
        }


        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method); // 

            request.AddHeader("Content-Type", baseRequest.ContentType); // 添加请求头，第一个参数为请求头名称，第二个参数为请求头值

            if (baseRequest.Parameter != null) // 如果请求参数不为空，添加请求参数
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody); // 序列化请求参数；定义参数类型为请求体

            client.BaseUrl = new Uri(apiUrl + baseRequest.Route); // 组合请求地址

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content); // 反序列化响应内容
            }
            else
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = $"Error: {response.StatusCode} - {response.ErrorMessage}"
                };
            }
        }
    }
}
