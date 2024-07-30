using LogisticsSystem.API.Service;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeaTransportDetailController : ControllerBase
    {
        private readonly ISeaTransportDetailService service;

        public SeaTransportDetailController(ISeaTransportDetailService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter parameter) => await service.GetAllAsync(parameter);

        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await service.GetSingleAsync(id);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] SeaTransportDetailDto model) => await service.AddAsync(model);

        [HttpPut]
        public async Task<ApiResponse> Update([FromBody] SeaTransportDetailDto model) => await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await service.DeleteAsync(id);
    }
}
